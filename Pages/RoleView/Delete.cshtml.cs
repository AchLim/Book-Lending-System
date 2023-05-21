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

namespace Book_Lending_System.Pages.RoleView
{
    public class DeleteModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DeleteModel(Book_Lending_System.Data.Book_Lending_SystemContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);

            if (role == null)
            {
                return NotFound();
            }
            else 
            {
                Role = role;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }
            var role = await _context.Roles.FindAsync(id);

            if (role != null)
            {
                Role = role;
                await _roleManager.DeleteAsync(role);
            }

            return RedirectToPage("./Index");
        }
    }
}
