using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Models.Dto
{
    public class AuthenticationResponseDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        [JsonIgnore] public string RefreshToken { get; set; }

    }
}
