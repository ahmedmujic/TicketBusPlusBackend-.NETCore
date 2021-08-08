

using AuthService.Constants.IdentityConfiguration;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace AuthService.Configuration.IdentityConfiguration
{
    public class IdentityConfiguration
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetResourceApis()
        {
            return new List<ApiResource>
            {
                new ApiResource(name: InternalApis.TicketService, displayName: "TicketService API") { Scopes = new List<string>() { InternalApis.TicketService } },
                new ApiResource(name: InternalApis.BookingManagement, displayName: "BookingManagement API") { Scopes = new List<string>() { InternalApis.BookingManagement } }
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new[]
            {
                new ApiScope(name: InternalApis.TicketService,   displayName: "TicketService Api Access"),
                new ApiScope(name: InternalApis.BookingManagement,   displayName: "BookingManagement Api Access")
            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client()
                {
                    ClientId = InternalClients.Web,
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = { new Secret(Environment.GetEnvironmentVariable("web_client_secret").Sha256()) },
                    AllowedScopes = new List<string>
                    {
                        InternalApis.TicketService,
                        InternalApis.BookingManagement,
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        IdentityServerConstants.StandardScopes.OfflineAccess
                    },
                    AllowOfflineAccess = true,
                    RefreshTokenUsage = TokenUsage.OneTimeOnly
                }
            };
        }
    }
}
