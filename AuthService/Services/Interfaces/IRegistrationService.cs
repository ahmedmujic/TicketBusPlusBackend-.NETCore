using AuthService.Models.Dto;
using AuthService.Models.Dto.Request;
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
        Task<IdentityResult> ActivateAccountAsync(ActivateAccountRequestDTO request);
        Task<IdentityResult> ResendActivationMailAsync(string email);
    }
}
