using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.RoleView
{
    public class CreateModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public CreateModel(Book_Lending_System.Data.Book_Lending_SystemContext context, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _roleManager = roleManager;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public string RoleName { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Roles == null || RoleName == null)
            {
                return Page();
            }

            IdentityRole? role = await _roleManager.FindByNameAsync(RoleName);
            if (role != null)
            {
                ModelState.AddModelError("", "Duplicated role found, please use different name.");
                return Page();
            }    

            await _roleManager.CreateAsync(new IdentityRole(RoleName));

            return RedirectToPage("./Index");
        }
    }
}
