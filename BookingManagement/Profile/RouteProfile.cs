using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BookingManagement.Models.Domain;
using BookingManagement.Models.DTO.Route;
using BookingManagement.Models.DTO.Route.Response;

namespace BookingManagement.Profile
{
    public class RouteProfile : AutoMapper.Profile
    {
        public RouteProfile()
        {
            CreateMap<Routes, AddRouteRequestDTO>();
        }
    }
}
