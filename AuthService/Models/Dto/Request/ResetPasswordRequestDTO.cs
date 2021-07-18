using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models.Dto.Request
{
    public class ResetPasswordRequestDTO
    {
        [Required]
        public string Id { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
