using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models.Dto
{
    public class RevokeTokenRequestDto
    {
        public string Token { get; set; }
    }
}
