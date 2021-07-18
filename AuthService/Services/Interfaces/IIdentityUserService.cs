using AuthService.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthService.Services.Interfaces
{
    public interface IIdentityUserService<TUser> where TUser : User
    {
        Task<TUser> FindByNameAsync(string userName);
        Task<IList<Claim>> GetClaimsAsync(TUser user);
        Task<IList<string>> GetRolesAsync(TUser user);
        PasswordVerificationResult VerifyHashedPassword(TUser user, string password);
    }
}
