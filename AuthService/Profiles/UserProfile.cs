using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Events;
using AuthService.Models;
using AuthService.Models.Domain;
using AuthService.Models.Dto;
using AuthService.Models.Dto.Response;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<UserRegistered, User>();
            CreateMap<RoleDto, Role>();
            CreateMap<User, UserInfoResponseDTO>();
        }
    }
}
