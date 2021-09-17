
using BookingManagement.Services.Bus;
using BookingManagement.Services.Bus.Interface;
using BookingManagement.Services.Routes;
using BookingManagement.Services.Seat;
using BookingManagement.Services.Seat.Interface;
using BookingManagement.Services.Station;
using BookingManagement.Services.Station.Interface;
using BookingManagement.Services.Ticket;
using BookingManagement.Services.Ticket.Interface;
using BookingManagement.Services.Town;
using BookingManagement.Services.Town.Interfaces;
using Messaging.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace BookingManagement.Extensions
{
    public static class ServicesExtension
    {

        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
        {
            services.UseRabbitMQMessagePublisher(config);

            services.AddScoped<ITownService, TownService>();
            services.AddScoped<IBusService, BusService>();
            services.AddScoped<IStationService, StationService>();
            services.AddScoped<IRouteService, RouteService>();
            services.AddScoped<ITicketService, Services.Ticket.TicketService>();
            services.AddScoped<ISeatService, SeatService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
