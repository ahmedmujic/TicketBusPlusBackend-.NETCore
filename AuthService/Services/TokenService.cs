using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using AuthService.Constants.Errors;
using AuthService.Constants.Identity;
using AuthService.Constants.TokenProviders;
using AuthService.Events;
using AuthService.Models;
using AuthService.Models.Domain;
using AuthService.Models.Dto;
using AuthService.Models.Dto.Request;
using AuthService.Models.Settings;
using AuthService.Services.Interfaces;
using IdentityModel.Client;
using Messaging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AuthService.Services
{
    public class TokenService : ITokenService
    {
        private readonly IMessagePublisher _messagePublisher;
        private readonly IOptions<EmailSettings> _emailSettings;
        private readonly ILogger<TokenService> _logger;
        private readonly IHttpClientFactory _clientFactory;
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _config;

        public TokenService(IHttpClientFactory clientFactory,
            IConfiguration config,
            UserManager<User> userManager,
            ILogger<TokenService> logger,
            IOptions<EmailSettings> emailSettings,
            IMessagePublisher messagePublisher)
        {
            _messagePublisher = messagePublisher;
            _emailSettings = emailSettings;
            _logger = logger;
            _clientFactory = clientFactory;
            _userManager = userManager;
            _config = config;
        }


        public async Task<TokenResponse> CreateTokenAsync(TokenRequestDTO request)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var cache = new DiscoveryCache(_config["AuthApiUrl"]);
                var disco = await cache.GetAsync()
                     .ConfigureAwait(false);
                if (disco.IsError)
                    throw new Exception(disco.Error);
                switch (request.GrantType)
                {
                    case GrantTypes.Password:
                        var passwordFlow = await client.RequestPasswordTokenAsync(new PasswordTokenRequest()
                        {
                            Address = disco.TokenEndpoint,
                            ClientId = request.ClientId,
                            ClientSecret = request.ClientSecret,
                            Scope = request.Scope,
                            UserName = request.Username,
                            Password = request.Password
                        }).ConfigureAwait(false);

                        return passwordFlow;
                    default:
                        return null;
                }

            }
            catch(Exception e)
            {
                _logger.LogError(e, nameof(CreateTokenAsync));
                throw;
            }
        }

        public async Task<TokenRevocationResponse> RevokeTokenAsync(RefreshTokenRequestDTO tokenRequest)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var cache = new DiscoveryCache(_config["AuthApiUrl"]);
                var disco = await cache.GetAsync()
                                       .ConfigureAwait(false);

                if (disco.IsError)
                    throw new Exception(disco.Error);

                var revokeResult = await client.RevokeTokenAsync(new TokenRevocationRequest
                {
                    Address = disco.RevocationEndpoint,
                    ClientId = tokenRequest.ClientId,
                    ClientSecret = tokenRequest.ClientSecret,
                    Token = tokenRequest.RefreshToken
                }).ConfigureAwait(false);

                return revokeResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RevokeTokenAsync));
                throw;
            }
        }

        public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequestDTO tokenRequest)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var cache = new DiscoveryCache(_config["AuthApiUrl"]);
                var disco = await cache.GetAsync()
                                       .ConfigureAwait(false);
                if (disco.IsError)
                    throw new Exception(disco.Error);

                var refreshToken = await client.RequestRefreshTokenAsync(new RefreshTokenRequest()
                {
                    Address = disco.TokenEndpoint,
                    ClientId = tokenRequest.ClientId,
                    ClientSecret = tokenRequest.ClientSecret,
                    RefreshToken = tokenRequest.RefreshToken
                }).ConfigureAwait(false);

                return refreshToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(RefreshTokenAsync));
                throw;
            }
        }

        public async Task<IdentityResult> RequestResetPassswordAsync(RequestResetPasswordDTO request)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email).ConfigureAwait(false);

                if (user is null)
                    return IdentityResult.Failed(new IdentityError
                    {
                        Code = ErrorCodes.UserNotFound,
                        Description = ErrorDescriptions.UserNotFoundEmail
                    });

                var result = await _userManager.UpdateSecurityStampAsync(user).ConfigureAwait(false);

                if (result.Succeeded)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user).ConfigureAwait(false);
                    token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                    var resetPasswordLink =
                        $"{_emailSettings.Value.FrontendAppUrl}/user/login/reset-password?userId={user.Id}&token={token}";

                    ResetPassword e = new ResetPassword(new Guid(), user.Id, user.FirstName, user.Email, resetPasswordLink);

                    await _messagePublisher.PublishMessageAsync(e.MessageType, e, "");
                }
                return result;            

            }catch(Exception ex)
            {
                _logger.LogError(ex, nameof(RequestResetPassswordAsync));
                throw;
            }
        }

        public async Task<IdentityResult> ValidateResetPasswordTokenAsync(PasswordTokenValidationDTO request)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(request.Id).ConfigureAwait(false);

                string decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.Token));

                var result = await _userManager.VerifyUserTokenAsync(user, TokenProviders.PasswordTokenProvider, UserManager<User>.ResetPasswordTokenPurpose, decodedToken).ConfigureAwait(false);

                var identityErrorDescriber = new IdentityErrorDescriber();

                return result ? IdentityResult.Success :
                IdentityResult.Failed(new IdentityError[]
                {
                    identityErrorDescriber.InvalidToken()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, nameof(ValidateResetPasswordTokenAsync));
                throw;
            }
        }
    }
}

