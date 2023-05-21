﻿using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Book_Lending_System.Data
{
    public class AccessRight
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccessRight(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<bool> IsAdmin(ClaimsPrincipal loggedUser)
        {
            if (loggedUser == null)
                return false;

            IdentityUser? currentUser = await _userManager.GetUserAsync(loggedUser);
            if (currentUser == null)
                return false;

            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, Data.Enum.Roles.Admin.ToString());
            return isAdmin;
        }

        public async Task<bool> IsStaff(ClaimsPrincipal loggedUser)
        {
            if (loggedUser == null)
                return false;

            IdentityUser? currentUser = await _userManager.GetUserAsync(loggedUser);
            if (currentUser == null)
                return false;

            bool isStaff = await _userManager.IsInRoleAsync(currentUser, Data.Enum.Roles.Staff.ToString());
            return isStaff;
        }
    }

}
