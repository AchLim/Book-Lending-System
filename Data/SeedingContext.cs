using Microsoft.AspNetCore.Identity;
using Book_Lending_System.Data.Enum;
using System;

namespace Book_Lending_System.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            var admin = Roles.Admin.ToString();
            var staff = Roles.Staff.ToString();
            var user = Roles.User.ToString();

            if (await roleManager.FindByNameAsync(admin) == null)
                await roleManager.CreateAsync(new IdentityRole(admin));

            if (await roleManager.FindByNameAsync(staff) == null)
                await roleManager.CreateAsync(new IdentityRole(staff));

            if (await roleManager.FindByNameAsync(user) == null)
                await roleManager.CreateAsync(new IdentityRole(user));
        }

        public static async Task SeedSuperAdminAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new IdentityUser
            {
                UserName = "admin",
                Email = "admin@example.com",
            };
            if (!userManager.Users.Any(u => u.UserName!.ToUpper() == defaultUser.UserName.ToUpper()))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123456");
                    await userManager.AddToRoleAsync(defaultUser, Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.Staff.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Roles.User.ToString());
                }

            }
        }
    }
}
