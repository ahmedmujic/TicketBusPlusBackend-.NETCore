using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models.Dto;

namespace AuthService.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<AuthenticationResponseDto> AuthenticateUserAsync(AuthenticationDto authenticationDto, string ipAddress);
        public Task<AuthenticationResponseDto> RefreshTokenAsync(string token, string ipAddress);
    }
}
