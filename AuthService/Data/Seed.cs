using AuthService.Models;
using AuthService.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AuthService.Data
{
    public class Seed
    {
        public static async Task SeedUsers(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            Console.WriteLine("Uslo");

            if (await userManager.Users.AnyAsync()) return;

           /* var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<User>>(userData);*/

            var roles = new List<Role>
            {
                new Role{Name = "Member"},
                new Role{Name = "Admin"},
                new Role{Name = "Moderator"},
            };

            foreach (var role in roles)
            {
                await roleManager.CreateAsync(role);
            }            

            var admin = new User
            {
                UserName = "admin",
                Email = "ahmedmujic123@gmail.com",
                EmailConfirmed = true,               
            };

            var createdAdmin = await userManager.CreateAsync(admin, "admin123");

            if (createdAdmin.Succeeded)
                await userManager.AddToRolesAsync(admin, new[] { "Admin", "Member", "Moderator" });
            else
                Console.WriteLine("jbg nije htjelo");
        }
    }
}
