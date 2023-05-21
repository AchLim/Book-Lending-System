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

namespace Book_Lending_System.Pages.AccountView
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<EditModel> _logger;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context,
                         UserManager<IdentityUser> userManager,
                         RoleManager<IdentityRole> roleManager,
                         ILogger<EditModel> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        [BindProperty]
        public IdentityUser UserAccount { get; set; } = default!;

        public List<SelectListItem> RolesList { get; set; } = default!;

        [BindProperty]
        public ICollection<string> SelectedRoleIds { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Users == null)
            {
                return NotFound();
            }

            var user =  await _context.Users.FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }
            UserAccount = user;

            if (_context.Roles != null && _context.UserRoles != null)
            {
                RolesList = new();
                var roles = _context.Roles.ToList();

                foreach (var role in roles)
                {
                    bool userHasRole = _context.UserRoles.Any(ur => ur.UserId == user.Id && ur.RoleId == role.Id);
                    var roleSelect = new SelectListItem(role.Name, role.Id, userHasRole);
                    RolesList.Add(roleSelect);
                }
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync(string id)
        {
            IdentityUser? user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                ModelState.AddModelError("", "Role is not found, it may have been modified on the same time.");
                return Page();
            }

            user.UserName = UserAccount.UserName;
            user.Email = UserAccount.Email;

            try
            {
                // Check for removed roles
                var userRoleIds = _context.UserRoles.Where(ur => ur.UserId == user.Id).Select(ur => ur.RoleId).ToList();
                foreach (var roleId in userRoleIds)
                {
                    if (!SelectedRoleIds.Contains(roleId))
                    {
                        IdentityRole role = (await _roleManager.FindByIdAsync(roleId))!;
                        await _userManager.RemoveFromRoleAsync(user, role.Name!);
                    }
                }

                // Directly add role to user.
                foreach (var r in SelectedRoleIds)
                {
                    IdentityRole roleResult = (await _roleManager.FindByIdAsync(r))!;
                    await _userManager.AddToRoleAsync(user, roleResult.Name!);
                }

                // Update user.
                IdentityResult result = await _userManager.UpdateAsync(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(UserAccount.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = UserAccount.Id });
        }

        private bool UserExists(string id)
        {
          return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
