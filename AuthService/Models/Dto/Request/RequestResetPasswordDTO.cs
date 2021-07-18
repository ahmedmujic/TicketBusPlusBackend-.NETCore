using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models.Dto.Request
{
    public class RequestResetPasswordDTO
    {
        [Required]
        public string Email { get; set; }
    }
}
