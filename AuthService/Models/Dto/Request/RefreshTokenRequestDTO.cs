using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Models.Dto.Request
{
    public class RefreshTokenRequestDTO
    {
        [Required]
        public string ClientId { get; set; }

        [Required]
        public string ClientSecret { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
    }
}
