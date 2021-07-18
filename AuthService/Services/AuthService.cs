using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Models.Dto;
using AuthService.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly ITokenService _tokenService;

        public AuthService(UserManager<User> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }


        public async Task<AuthenticationResponseDto> AuthenticateUserAsync(AuthenticationDto authenticationDto, string ipAddress)
        {
            var userFromDb = await _userManager.Users.Where(u =>u.Email == authenticationDto.Email).Include(u=> u.RefreshTokens).FirstOrDefaultAsync();
            var isPasswordOk = await _userManager.CheckPasswordAsync(userFromDb, authenticationDto.Password);

            if (userFromDb is null || !isPasswordOk)
            {
                return null;
            }

            var refreshToken = _tokenService.GenerateRefreshToken(ipAddress);
            userFromDb.RefreshTokens.Add(refreshToken);
            await _userManager.UpdateAsync(userFromDb);

            return new AuthenticationResponseDto
            {
                Email = userFromDb.Email,
                Token = await _tokenService.CreateToken(userFromDb),
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticationResponseDto> RefreshTokenAsync(string token, string ipAddress)
        {
            var user = await _userManager.Users.Include(u => u.RefreshTokens).SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == token));

            if (user is null)
                return null;

            foreach (var userRefreshToken in user.RefreshTokens)
            {
                Console.WriteLine(userRefreshToken);
            }
           
            
            var refreshToken = user.RefreshTokens.Single(rt => rt.Token == token);

            if (!refreshToken.IsActive)
                return null;
            
            var jwtToken = await _tokenService.CreateToken(user);

            return new AuthenticationResponseDto
            {
                Token = jwtToken,
                Email = user.Email
            };
        }
    }
}
