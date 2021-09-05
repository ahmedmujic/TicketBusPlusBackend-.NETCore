
using BookingManagement.Services.Bus;
using BookingManagement.Services.Bus.Interface;
using BookingManagement.Services.Routes;
using BookingManagement.Services.Station;
using BookingManagement.Services.Station.Interface;
using BookingManagement.Services.Town;
using BookingManagement.Services.Town.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookingManagement.Extensions
{
    public static class ServicesExtension
    {

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
           

            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IBusService, BusService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
