using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Models.Domain;

namespace AuthService.Services.Interfaces
{
    public interface ITokenService
    {
        public Task<string> CreateToken(User user);
        RefreshToken RefreshToken(string ipAddress, RefreshToken refreshToken);
        RefreshToken GenerateRefreshToken(string ipAddress);

        Task<bool> RevokeToken(string token, string ipAddress);
    }
}
