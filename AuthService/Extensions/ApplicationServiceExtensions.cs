
using AuthService.Interfaces;
using AuthService.Models;
using AuthService.repositories;
using AuthService.repositories.interfaces;
using AuthService.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Services.Interfaces;
using Messaging.Configuration;

namespace AuthService.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.UseRabbitMQMessagePublisher(config);

            services.AddScoped<IRegistrationService, RegistrationService>();
            services.AddScoped<IRegistrationRepository, RegistrationRepository>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAdminService, AdminService>();

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
