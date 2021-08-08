using AuthService.Constants.Errors;
using AuthService.Models;
using AuthService.Services.Interfaces;
using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Services
{
        public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
        {
            private readonly IIdentityUserService<User> _identityUserService;
            private readonly ILogger<ResourceOwnerPasswordValidator> _logger;

            public ResourceOwnerPasswordValidator(IIdentityUserService<User> identityUserService,
                                                  ILogger<ResourceOwnerPasswordValidator> logger)
            {
                _identityUserService = identityUserService;
                _logger = logger;
            }

            public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
            {
                try
                {
                    var user = await _identityUserService.FindByNameAsync(context.UserName)
                                                         .ConfigureAwait(false);
                    if (user != null)
                    {
                        var verificationResult = _identityUserService.VerifyHashedPassword(user, context.Password);
                        if (verificationResult == PasswordVerificationResult.Success)
                        {
                            if (!user.EmailConfirmed)
                            {
                                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidRequest, ErrorDescriptions.AccountNotConfirmed);
                                return;
                            }

                            List<Claim> userClaims = (await _identityUserService.GetClaimsAsync(user)
                                                                                    .ConfigureAwait(false)).ToList();
                            //Add role claims for role authorization
                            foreach (var role in await _identityUserService.GetRolesAsync(user).ConfigureAwait(false))
                                userClaims.Add(new Claim(JwtClaimTypes.Role, role));

                            context.Result = new GrantValidationResult(
                                subject: user.Id,
                                authenticationMethod: "custom",
                                claims: userClaims);
                            return;
                        }

                        context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect credentials.");
                        return;
                    }

                    context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "Incorrect credentials.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(ValidateAsync));
                    throw;
                }
            }
        }
}
