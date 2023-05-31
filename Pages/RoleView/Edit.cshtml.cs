using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Book_Lending_System.FileUploadService;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.RoleView
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context, RoleManager<IdentityRole> roleManager, ILogger<EditModel> logger)
        {
            _context = context;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public IdentityRole Role { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Roles == null)
            {
                return NotFound();
            }

            var role =  await _context.Roles.FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }
            Role = role;

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Role.Name))
            {
                ModelState.AddModelError("", "Role name cannot be empty.");
                return Page();
            }

            IdentityRole? duplicatedRole = await _roleManager.FindByNameAsync(Role.Name);
            if (duplicatedRole != null && duplicatedRole.Id != id)
            {
                ModelState.AddModelError("", "Duplicated role found, please use different name.");
                return Page();
            }

            IdentityRole? role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                ModelState.AddModelError("", "Role is not found, it may have been modified on the same time.");
                return Page();
            }

            role.Name = Role.Name;

            try
            {
                IdentityResult result = await _roleManager.UpdateAsync(role);
                if (!result.Succeeded)
                {
                    List<string> errorList = new();
                    foreach (var error in result.Errors)
                    {
                        errorList.Add($"{error.Code}: {error.Description}");
                    }

                    ModelState.AddModelError("", string.Join('\n', errorList));
                    return Page();
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(Role.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = Role.Id });
        }

        private bool RoleExists(string id)
        {
          return (_context.Roles?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
