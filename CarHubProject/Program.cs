using CarHubProject.Models;
using CarHubProject.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run(); 