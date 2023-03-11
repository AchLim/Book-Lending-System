using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Book_Lending_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Book_Lending_System.Models;
using Book_Lending_System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/Index");
});

builder.Services.AddDbContext<Book_Lending_SystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Book_Lending_SystemContext") ?? throw new InvalidOperationException("Connection string 'Book_Lending_SystemContext' not found.")));

builder.Services.AddTransient<IEmailSender, EmailSender>();
//builder.Services.AddIdentity<Account, Role>().AddEntityFrameworkStores<Book_Lending_SystemContext>().AddDefaultTokenProviders();
builder.Services.AddDefaultIdentity<Account>(
    options => options.SignIn.RequireConfirmedAccount = true
).AddRoles<Role>().AddEntityFrameworkStores<Book_Lending_SystemContext>();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.AddControllers(config =>
{
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    config.Filters.Add(new AuthorizeFilter(policy));
});

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
