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
using System.Text;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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

            IdentityResult result = await _roleManager.CreateAsync(new IdentityRole(RoleName));
            if (!result.Succeeded)
            {
                StringBuilder sb = new();
                var errorList = result.Errors.ToList();
                foreach (var e in errorList)
                {
                    sb.Append(e.Description + '\n');
                }
                ModelState.AddModelError("", sb.ToString());
            }
            else
            {
                IdentityRole roleCreated = (await _roleManager.FindByNameAsync(RoleName))!;
                return RedirectToPage("./Details", new { id = roleCreated.Id });
            }
            return Page();
        }
    }
}
