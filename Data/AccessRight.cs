using Microsoft.AspNetCore.Identity;
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

        public async Task<string> GetUserName(ClaimsPrincipal loggedUser)
        {
            var user = await GetCurrentUser(loggedUser!);
            return user!.UserName!;
        }

        public async Task<bool> IsAdmin(ClaimsPrincipal loggedUser)
        {
            IdentityUser? currentUser = await GetCurrentUser(loggedUser);
            if (currentUser == null)
                return false;

            bool isAdmin = await _userManager.IsInRoleAsync(currentUser, Data.Enum.Roles.Admin.ToString());
            return isAdmin;
        }

        public async Task<bool> IsStaff(ClaimsPrincipal loggedUser)
        {
            IdentityUser? currentUser = await GetCurrentUser(loggedUser);
            if (currentUser == null) 
                return false;

            bool isStaff = await _userManager.IsInRoleAsync(currentUser, Data.Enum.Roles.Staff.ToString());
            return isStaff;
        }

        public async Task<IdentityUser?> GetCurrentUser(ClaimsPrincipal loggedUser)
        {
            if (loggedUser == null)
                return null;

            IdentityUser? currentUser = await _userManager.GetUserAsync(loggedUser);
            if (currentUser == null)
                return null;

            return currentUser;
        }
    }

}
