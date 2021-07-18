using AuthService.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Interfaces
{
    public interface IRegistrationService
    {
        Task<IdentityResult> Register(UserDto user);
        Task<bool> ConfirmEmail(string userId, string token); 
    }
}
