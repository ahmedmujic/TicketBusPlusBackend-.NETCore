using AuthService.Models.Domain;
using AuthService.Models.Enum.User;
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
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Gender Gender { get; set; }
        public string CeoFirstName { get; set; }
        public string CeoLastName { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }
        public DateTime RegistrationDate { get; set; }
        public bool IsBlocked { get; set; }
        public ICollection<ApplicationUserRole> UserRoles { get; set; }

    }
}
