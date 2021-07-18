using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly ILogger<IdentityProfileService> _logger;

        public IdentityProfileService(ILogger<IdentityProfileService> logger)
        {
            _logger = logger;
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                var identity = (ClaimsIdentity)context.Subject.Identity;

                context.IssuedClaims.AddRange(identity.Claims);
                return Task.FromResult(0);
            }
            catch(Exception e)
            {
                _logger.LogError(e, nameof(GetProfileDataAsync));
                throw;
            }
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            throw new NotImplementedException();
        }
    }
}
