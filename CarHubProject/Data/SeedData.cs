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
                var anotherUser = new User
                {
                    UserName = "customer@carhub.com",
                    Email = "customer@carhub.com",
                    EmailConfirmed = true,
                    FirstName = "Customer",
                    LastName = "User"
                };

                await userManager.CreateAsync(anotherUser, "User@123");
                await userManager.AddToRoleAsync(anotherUser, "User");
            }

            if (!context.Brands.Any())
            {
                var brands = new Brand[]
                {
                    new Brand{BrandName="Toyota"},
                    new Brand{BrandName="Ford"},
                    new Brand{BrandName="Honda"},
                    new Brand{BrandName="BMW"},
                    new Brand{BrandName="Mercedes-Benz"},
                    new Brand{BrandName="Tesla"}
                };
                foreach (Brand b in brands)
                {
                    context.Brands.Add(b);
                }
                await context.SaveChangesAsync();
            }

            if (!context.Cars.Any())
            {
                var toyota = context.Brands.First(b => b.BrandName == "Toyota");
                var ford = context.Brands.First(b => b.BrandName == "Ford");
                var honda = context.Brands.First(b => b.BrandName == "Honda");
                var bmw = context.Brands.First(b => b.BrandName == "BMW");
                var mercedes = context.Brands.First(b => b.BrandName == "Mercedes-Benz");
                var tesla = context.Brands.First(b => b.BrandName == "Tesla");

                var cars = new Car[]
                {
                    new Car{
                        Model="Corolla",
                        Year=2023,
                        PricePerDay=140.00m,
                        SalePrice=200000.00m,
                        Status="Available",
                        Color="Red",
                        Transmission="Automatic",
                        FuelType="Petrol",
                        Mileage=12000,
                        BrandId=toyota.BrandId,
                        ImageUrl="/uploads/cars/toyota-corolla.jpg"
                    },
                    new Car{
                        Model="Mustang",
                        Year=2022,
                        PricePerDay=180.00m,
                        SalePrice=400000.00m,
                        Status="Available",
                        Color="Blue",
                        Transmission="Manual",
                        FuelType="Petrol",
                        Mileage=20000,
                        BrandId=ford.BrandId,
                        ImageUrl="/uploads/cars/ford-mustang.jpg"
                    },
                    new Car{
                        Model="Civic",
                        Year=2023,
                        PricePerDay=145.00m,
                        SalePrice=220000.00m,
                        Status="Available",
                        Color="Silver",
                        Transmission="Automatic",
                        FuelType="Petrol",
                        Mileage=10000,
                        BrandId=honda.BrandId,
                        ImageUrl="/uploads/cars/honda-civic.jpg"
                    },
                    new Car{
                        Model="3 Series",
                        Year=2022,
                        PricePerDay=250.00m,
                        SalePrice=500000.00m,
                        Status="Available",
                        Color="Gray",
                        Transmission="Automatic",
                        FuelType="Petrol",
                        Mileage=18000,
                        BrandId=bmw.BrandId,
                        ImageUrl="/uploads/cars/bmw-3series.jpg"
                    },
                    new Car{
                        Model="C-Class",
                        Year=2023,
                        PricePerDay=280.00m,
                        SalePrice=550000.00m,
                        Status="Available",
                        Color="White",
                        Transmission="Automatic",
                        FuelType="Petrol",
                        Mileage=12000,
                        BrandId=mercedes.BrandId,
                        ImageUrl="/uploads/cars/mercedes-c-class.jpg"
                    },
                    new Car{
                        Model="Cybertruck",
                        Year=2023,
                        PricePerDay=500.00m,
                       SalePrice=1000000.00m,
                        Status="Available",
                        Color="Silver",
                        Transmission="Automatic",
                        FuelType="Electric",
                        Mileage=5000,
                        BrandId=tesla.BrandId,
                        ImageUrl="/uploads/cars/tesla-cybertruck.jpg"
                    }
                };

                foreach (var c in cars)
                    context.Cars.Add(c);

                await context.SaveChangesAsync();
            }

        }
    }
}
