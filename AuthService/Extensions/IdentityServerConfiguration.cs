using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Models.Domain;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace AuthService.Extensions
{
    public static class IdentityServerConfiguration
    {
        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            string identityConnectionString = config.GetConnectionString("IdentityServerBusDb");

            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Tokens.EmailConfirmationTokenProvider = "EmailDataProtectorTokenProvider";
            })
             .AddEntityFrameworkStores<AuthenticationDbContext>()
             .AddDefaultTokenProviders();

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddAspNetIdentity<User>()
                    //Configuration Store: clients and resources
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = db =>
                        db.UseSqlServer(identityConnectionString,
                            sql => sql.MigrationsAssembly(InternalAssemblies.Database));
                    })
                    //Operational Store: tokens, codes etc.
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = db =>
                        db.UseSqlServer(identityConnectionString,
                            sql => sql.MigrationsAssembly(InternalAssemblies.Database));
                    })
                    .AddProfileService<IdentityProfileService>(); // custom claims 

            //Cache Discovery document HttpClient
            services.AddSingleton<IDiscoveryCache>(r =>
            {
                var factory = r.GetRequiredService<IHttpClientFactory>();
                return new DiscoveryCache(config["AuthApiUrl"], () => factory.CreateClient());
            });
        }

        
    }
}
