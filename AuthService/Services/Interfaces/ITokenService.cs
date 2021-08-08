using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Models;
using AuthService.Models.Domain;
using AuthService.Models.Dto;
using AuthService.Models.Dto.Request;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;

namespace AuthService.Services.Interfaces
{
    public interface ITokenService
    {
        Task<TokenResponse> CreateTokenAsync(TokenRequestDTO request);
        Task<TokenRevocationResponse> RevokeTokenAsync(RefreshTokenRequestDTO tokenRequest);
        Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequestDTO tokenRequest);
        Task<IdentityResult> RequestResetPassswordAsync(RequestResetPasswordDTO request);
        Task<IdentityResult> ValidateResetPasswordTokenAsync(PasswordTokenValidationDTO request);
    }
}
