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

namespace AuthService.Services
{
    public class RegistrationService : IRegistrationService
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public ILogger<RegistrationService> _logger { get; }

        public RegistrationService(UserManager<User> userManager, IMapper mapper, IMessagePublisher messagePublisher, 
            ILogger<RegistrationService> logger)
        {
            _mapper = mapper;
            _messagePublisher = messagePublisher;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<bool> ConfirmEmail(string userId, string token)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return false;

            var decodedTokenString = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userManager.ConfirmEmailAsync(user, decodedTokenString);

            if (result.Succeeded)            
                return true;
            
            return false; 
        }

        public async Task<IdentityResult> RegisterUser(UserDto user)
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

                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(applicationUser);

                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    var confirmationLink = "http://localhost:4200/userId=" + applicationUser.Id + "&token=" + token;

                    UserRegistered e = new UserRegistered(new Guid(), applicationUser.Id, applicationUser.UserName, applicationUser.Email, confirmationLink);

                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, ""); 
                }

                return identityResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RegisterUser));
                throw;
            }

        }
    }
}
