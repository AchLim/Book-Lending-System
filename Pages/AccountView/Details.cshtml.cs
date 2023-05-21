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
    public class DetailsModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DetailsModel(Book_Lending_System.Data.Book_Lending_SystemContext context, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IdentityUser UserAccount { get; set; } = default!;

        public IList<string> UserRoles { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            UserAccount = user;

            if (_context.UserRoles != null)
            {
                UserRoles = await _userManager.GetRolesAsync(user);
            }

            return Page();
        }
    }
}
