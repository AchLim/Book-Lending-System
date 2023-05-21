using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.AccountView
{
    public class IndexModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IndexModel(Book_Lending_System.Data.Book_Lending_SystemContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;

            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IList<IdentityUser> Users { get;set; } = default!;

        public Dictionary<string, List<IdentityRole>> UserRolesDict { get; set; } = default!;
        public IEnumerable<IdentityRole> Roles { get; set; }

        public async Task OnGetAsync()
        {
            if (_context.Users != null)
            {
                Users = await _context.Users.Where(u => u.NormalizedUserName != "ADMIN").ToListAsync();
                UserRolesDict = new();

                foreach (IdentityUser user in Users)
                {
                    List<IdentityRole> roleList = new();
                    IList<string> roles = await _userManager.GetRolesAsync(user);
                    if (_context.Roles != null)
                    {
                        roleList = _context.Roles.Where(role => roles.Contains(role.Name!)).ToList();
                    }
                    UserRolesDict.Add(user.Id, roleList);
                }
            }
        }
    }
}
