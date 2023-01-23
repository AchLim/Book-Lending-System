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

namespace Book_Lending_System.Pages.UserAccounts
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserAccount UserAccount { get; set; } = default!;

        [BindProperty]
        public List<Role> Roles { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null || _context.UserAccount == null)
            {
                return NotFound();
            }

            var useraccount =  await _context.UserAccount.FirstOrDefaultAsync(m => m.Id == id);
            if (useraccount == null)
            {
                return NotFound();
            }
            UserAccount = useraccount;
            var role = _context.Role.Where(r => r.Id == id);
            if (role != null)
            {
                Roles = role.ToList();
            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(UserAccount).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserAccountExists(UserAccount.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            ICollection<Role> roles = UserAccount.GetRoleChanges(_context);

            IQueryable<Role> existingRoles = _context.Role.Where(r => r.UserAccountId == UserAccount.Id);
            if (existingRoles != null)
            {
                _context.Role.RemoveRange(existingRoles);
                await _context.SaveChangesAsync();
            }
            if (_context.Role != null)
            {
                await _context.Role.AddRangeAsync(roles);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }

        private bool UserAccountExists(uint id)
        {
          return (_context.UserAccount?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
