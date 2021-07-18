using AuthService.Models;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Services
{
        public class IdentityUserService<TUser> : IIdentityUserService<TUser> where TUser : User
        {
            private readonly UserManager<TUser> _userManager;
            private readonly ILogger<IdentityUserService<TUser>> _logger;

            public IdentityUserService(UserManager<TUser> userManager,
                                       ILogger<IdentityUserService<TUser>> logger)
            {
                _userManager = userManager;
                _logger = logger;
            }

            public async Task<TUser> FindByNameAsync(string userName)
            {
                try
                {
                    return await _userManager.FindByNameAsync(userName).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(FindByNameAsync));
                    throw;
                }
            }

            public async Task<IList<Claim>> GetClaimsAsync(TUser user)
            {
                try
                {
                    return await _userManager.GetClaimsAsync(user).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(GetClaimsAsync));
                    throw;
                }
            }

            public async Task<IList<string>> GetRolesAsync(TUser user)
            {
                try
                {
                    return await _userManager.GetRolesAsync(user).ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(GetRolesAsync));
                    throw;
                }
            }

            public PasswordVerificationResult VerifyHashedPassword(TUser user, string password)
            {
                try
                {
                    return _userManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, nameof(VerifyHashedPassword));
                    throw;
                }
            }
        }
    }

