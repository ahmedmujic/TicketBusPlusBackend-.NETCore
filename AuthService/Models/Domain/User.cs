using AuthService.Models.Domain;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Models
{
    public class User : IdentityUser
    {
        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; set; }

        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
