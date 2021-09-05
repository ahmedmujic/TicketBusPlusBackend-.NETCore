using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AuthService.Models.Dto.Response
{
    public class UserInfoResponseDTO
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string CeoFirstName { get; set; }
        public string CeoLastName { get; set; }
    }
}
