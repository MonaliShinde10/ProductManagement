using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProductManagement.Models;
using ProductManagement.Data;
using Microsoft.EntityFrameworkCore;
using ProductManagement.Controllers;
using ProductManagement.Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using System;
using ProductManagement.Services;
using ProductManagement.Models.ViewModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddAuthentication();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ISuperAdminService, SuperAdminService>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configure the authorization policies

/*builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SuperAdminPolicy", policy => policy.RequireRole("SuperAdmin"));
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("UserPolicy", policy => policy.RequireRole("User"));
});
*/

builder.Services.ConfigureApplicationCookie(options =>
{

    options.LoginPath = "/Account/Login";  
    options.LogoutPath = "/Account/logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.Services.AddAuthentication().AddCookie();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var serviceProvider = scope.ServiceProvider;
    SeedData.Initialize(serviceProvider).Wait();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Account",
    pattern: "{controller=Account}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "product",
    pattern: "{controller=Product}/{action=AllProducts}/{id?}");

app.MapControllerRoute(
    name: "Admin",
    pattern: "{controller=User}/{action=UserDashboard}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
