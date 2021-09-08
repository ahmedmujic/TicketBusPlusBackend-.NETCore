using AuthService.Models.Enum.User;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthService.Models.Dto
{
    public class UserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Gender Gender { get; set; } = 0;
        public string CeoFirstName { get; set; }
        public string CeoLastName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
    }
}
