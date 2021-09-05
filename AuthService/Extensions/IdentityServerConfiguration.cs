using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AuthService.Constants.Assemblies;
using AuthService.Constants.TokenProviders;
using AuthService.Extensions.Providers;
using AuthService.Models;
using AuthService.Models.Domain;
using AuthService.Services;
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
        public static void  AddIdentityServices(this IServiceCollection services, IConfiguration config)
        {
            string identityConnectionString = config.GetConnectionString("IdentityDbContext");

            services.AddIdentity<User, Role>(options =>
            {
                options.User.RequireUniqueEmail = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Tokens.PasswordResetTokenProvider = TokenProviders.PasswordTokenProvider;
            })
             .AddEntityFrameworkStores<AuthenticationDbContext>()
              .AddDefaultTokenProviders()
             .AddTokenProvider<PasswordResetTokenProvider<User>>(TokenProviders.PasswordTokenProvider);
            

            services.AddIdentityServer()
                    .AddDeveloperSigningCredential()
                    .AddAspNetIdentity<User>()
                    //Configuration Store: clients and resources
                    .AddConfigurationStore(options =>
                    {
                        options.ConfigureDbContext = db =>
                        db.UseSqlServer(identityConnectionString,
                            sql => sql.MigrationsAssembly(InternalAsseblies.Database));
                    })
                    //Operational Store: tokens, codes etc.
                    .AddOperationalStore(options =>
                    {
                        options.ConfigureDbContext = db =>
                        db.UseSqlServer(identityConnectionString,
                            sql => sql.MigrationsAssembly(InternalAsseblies.Database));
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
