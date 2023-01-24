using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Book_Lending_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<Book_Lending_SystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Book_Lending_SystemContext") ?? throw new InvalidOperationException("Connection string 'Book_Lending_SystemContext' not found.")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<Book_Lending_SystemContext>().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
