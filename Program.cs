using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Book_Lending_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Book_Lending_System.Models;
using Book_Lending_System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Book_Lending_System.FileUploadService;
using Microsoft.Extensions.Options;
using Quartz;
using Book_Lending_System.Data.Jobs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages(options =>
{
    options.Conventions.AllowAnonymousToPage("/Index");
});

builder.Services.AddDbContext<Book_Lending_SystemContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Book_Lending_SystemContext") ?? throw new InvalidOperationException("Connection string 'Book_Lending_SystemContext' not found.")));

builder.Services.AddTransient(typeof(AccessRight));
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddScoped<IFileUploadService, LocalFileUploadService>();
builder.Services.AddDefaultIdentity<IdentityUser>(
    options =>
    {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireDigit = false;
        options.Password.RequiredLength = 6;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequireLowercase = false;
    }
).AddRoles<IdentityRole>().AddEntityFrameworkStores<Book_Lending_SystemContext>()
.AddDefaultUI()
.AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
});

builder.Services.AddControllersWithViews();

// Every minute => 0 * * ? * *
// Every hour => 0 0 * ? * *
// Every day on 3 pm => 0 0 15 * * ?
// Fire at 10:15 am => 0 15 10 * * ?
// Fire at 22:10 pm => 0 10 22 * * ?

builder.Services.AddQuartz(q =>
{
    q.UseMicrosoftDependencyInjectionJobFactory();
    var jobKey = new JobKey("EmailSchedulerJob");
    q.AddJob<EmailSchedulerJob>(opts => opts.WithIdentity(jobKey));

    q.AddTrigger(opts => opts
                         .ForJob(jobKey)
                         .WithIdentity("EmailScheduler-trigger")
                         .WithCronSchedule("0 01 22 * * ?"));
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

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

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var loggerFactory = services.GetRequiredService<ILoggerFactory>();
    try
    {
        var context = services.GetRequiredService<Book_Lending_SystemContext>();
        var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
        await ContextSeed.SeedRolesAsync(userManager, roleManager);
        await ContextSeed.SeedSuperAdminAsync(userManager, roleManager);
    }
    catch (Exception ex)
    {
        var logger = loggerFactory.CreateLogger<Program>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

app.Run();
