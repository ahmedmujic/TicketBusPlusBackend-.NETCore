using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models.Dto.Response
{
    public class TokenResponseDTO
    {
        public string Token { get; set; }
        public int ExpiresIn { get; set; }
    }
}
