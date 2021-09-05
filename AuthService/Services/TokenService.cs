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
using AuthService.Models.Dto.Response;
using AuthService.Models.Settings;
using AuthService.Services.Interfaces;
using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly AuthenticationDbContext _dbContext;

        public TokenService(IHttpClientFactory clientFactory,
            IConfiguration config,
            UserManager<User> userManager,
            ILogger<TokenService> logger,
            IOptions<EmailSettings> emailSettings,
            IMessagePublisher messagePublisher,
            IMapper mapper,
            AuthenticationDbContext dbContext)
        {
            _messagePublisher = messagePublisher;
            _emailSettings = emailSettings;
            _logger = logger;
            _clientFactory = clientFactory;
            _userManager = userManager;
            _config = config;
            _mapper = mapper;
            _dbContext = dbContext;
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
                        $"{_emailSettings.Value.FrontendAppUrl}/auth/reset-password?userId={user.Id}&token={token}";

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

        public async Task<UserInfoResponseDTO> GetUserInfoAsync(string userId, string role)
        {
            try
            {
                
                var result = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);

                var mapped = _mapper.Map<UserInfoResponseDTO>(result);

                return mapped; 
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, nameof(GetUserInfoAsync));
                throw;
            }
        }
    }
}

