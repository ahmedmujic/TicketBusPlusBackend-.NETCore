using AuthService.Interfaces;
using AuthService.Models;
using AuthService.Models.Dto;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Events;
using AutoMapper;
using Messaging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using AuthService.Models.Settings;
using AuthService.Models.Dto.Request;
using AuthService.Constants.Errors;

namespace AuthService.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public ILogger<RegistrationService> _logger { get; }

        public RegistrationService(UserManager<User> userManager,
            IMapper mapper,
            IMessagePublisher messagePublisher, 
            ILogger<RegistrationService> logger,
            IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings;
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IdentityResult> Register(UserDto user)
        {
            try
            {
                User applicationUser = _mapper.Map<UserDto, User>(user);
                var identityResult = await _userManager.CreateAsync(applicationUser, user.Password).ConfigureAwait(false);

                if (identityResult.Succeeded)
                {
                    await _userManager.AddClaimsAsync(applicationUser, new List<Claim>() {
                        new Claim("email", applicationUser.Email)
                }).ConfigureAwait(false);

                    await _userManager.AddToRolesAsync(applicationUser, new List<string>() { "Admin" })
                                      .ConfigureAwait(false); // Define user roles on registration

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser).ConfigureAwait(false);

                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    var confirmationLink = $"{_emailSettings.Value.FrontendAppUrl}/user/activate?userId=" + applicationUser.Id + "&token=" + token;

                    UserRegistered e = new UserRegistered(new Guid(), applicationUser.Id, applicationUser.FirstName, applicationUser.Email, confirmationLink);

                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                }

                return identityResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(Register));
                throw;
            }
        }

        public async Task<IdentityResult> ActivateAccountAsync(ActivateAccountRequestDTO request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id);

                if (user is null)
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ErrorCodes.UserNotFound,
                        Description = ErrorDescriptions.UserNotFoundEmail
                    });

                if (user.EmailConfirmed)
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ErrorCodes.AccountAlreadyConfirmed,
                        Description = ErrorDescriptions.AccountNotConfirmed
                    });

                string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

                var result = await _userManager.ConfirmEmailAsync(user, decodedToken).ConfigureAwait(false);

                return result;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(ActivateAccountAsync));
                throw;
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(ResetPasswordRequestDTO request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id).ConfigureAwait(false);

                if (user is null)
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ErrorCodes.UserNotFound,
                        Description = ErrorDescriptions.UserNotFoundEmail
                    });

                if (!user.EmailConfirmed)
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ErrorCodes.AccountNotConfirmed,
                        Description = ErrorDescriptions.AccountNotConfirmed
                    });

                var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

                var result = await _userManager.ResetPasswordAsync(user, decodedToken, request.Password).ConfigureAwait(false);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ResetPasswordAsync));
                throw;
            }
        }

        public async Task<IdentityResult> ResendActivationMailAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email).ConfigureAwait(false);

            if (user is null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = ErrorCodes.UserNotFound,
                    Description = ErrorDescriptions.UserNotFoundEmail
                });

            var result = await _userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);

            if (result.Succeeded)
            {
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user).ConfigureAwait(false);

                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var confirmationLink =
                    $"{_emailSettings.Value.FrontendAppUrl}/user/sign-up/activate-account?userId={user.Id}&token={token}";

                UserRegistered e = new UserRegistered(new Guid(), user.Id, user.FirstName, user.Email, confirmationLink);

                await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");

                return IdentityResult.Success;
            }
            return result;
        }
    }
}
