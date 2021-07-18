using BookingManagement.Models.Domain;
using BookingManagement.Models.DTO.Bus.Request;
using BookingManagement.Models.DTO.Bus.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookingManagement.Profile
{
    public class BusProfile : AutoMapper.Profile
    {
        public BusProfile()
        {
            CreateMap<Bus, AddBusRequestDTO>().ReverseMap().ForMember(
                dest => dest.Name, opts=> opts.MapFrom(src=> src.Name))
                .ForMember(dest => dest.NumberOfSeats, opts => opts.MapFrom(src => src.NumberOfSeats));
        }
    }
}
