using CarHubProject.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CarHubProject.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            await context.Database.EnsureCreatedAsync();

            // Look for any roles.
            if (!context.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
            }

            // Look for any users.
            if (!context.Users.Any())
            {
                var adminUser = new User
                {
                    UserName = "admin@carhub.com",
                    Email = "admin@carhub.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                await userManager.CreateAsync(adminUser, "Admin@123");
                await userManager.AddToRoleAsync(adminUser, "Admin");

                var regularUser = new User
                {
                    UserName = "user@carhub.com",
                    Email = "user@carhub.com",
                    EmailConfirmed = true,
                    FirstName = "Regular",
                    LastName = "User"
                };

                await userManager.CreateAsync(regularUser, "User@123");
                await userManager.AddToRoleAsync(regularUser, "User");
            }
        }
    }
}
