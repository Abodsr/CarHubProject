using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Identity;

// Create one shared root for the in-memory database 
var inMemoryRoot = new InMemoryDatabaseRoot();
var builder = WebApplication.CreateBuilder(args);

// Add services to the container. 
builder.Services.AddControllersWithViews();

// Add DbContext to the container 
//This For Temproray DB To Test The App without Data Base if You Want To use Sql just Make useInMemory=false in appsettings 

// Register DbContext 


builder.Services.AddControllersWithViews();

bool useInMemory = builder.Configuration.GetValue<bool>("DatabaseSettings:UseInMemory");

if (useInMemory)
{
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseInMemoryDatabase("CarHubInMemory", inMemoryRoot),
        ServiceLifetime.Scoped);
    Console.WriteLine("? Using Shared In-Memory Database");
}
else
{
    builder.Services.AddDbContext<AppDbContext>(
        options => options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));
    Console.WriteLine("? Using SQL Server Database");
}

builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders()
    .AddDefaultUI();

// Register Repositories (Scoped) 
builder.Services.AddScoped<IBrandRepository, BrandRepository>();
builder.Services.AddScoped<ICarRepository, CarRepository>();
var app = builder.Build();


// Configure the HTTP request pipeline. 
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Car}/{action=Index}/{id?}");

app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        await CarHubProject.Data.SeedData.Initialize(services);
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run(); 