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
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.UserView
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserPartner UserPartner { get; set; } = default!;

        [BindProperty]
        public ICollection<IdentityUser> IdentityUsers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.UserPartner == null)
            {
                return NotFound();
            }

            var userpartner =  await _context.UserPartner.Include(up => up.User).FirstOrDefaultAsync(m => m.Id == id);
            if (userpartner == null)
            {
                return NotFound();
            }
            UserPartner = userpartner;

            if (_context.Users != null)
            {
                var users = await _context.Users.ToListAsync();
                var userPartners = await _context.UserPartner.Include(up => up.User).ToListAsync();

                IdentityUsers = new List<IdentityUser>();

                foreach (var user in users)
                    IdentityUsers.Add(user);

                foreach (var userPartner in userPartners)
                {
                    if (userPartner.Id != UserPartner.Id && userPartner.User != null)
                    {
                        IdentityUsers.Remove(userPartner.User);
                    }
                }

                //IdentityUsers = await _context.Users.ToListAsync();
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

            if (UserPartnerExists(UserPartner.NIK!))
            {
                ModelState.AddModelError(string.Empty, "NIK exists!");
                return Page();
            }

            _context.Attach(UserPartner).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserPartnerExists(UserPartner.Id!))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = UserPartner.Id });
        }

        private bool UserPartnerExists(string id)
        {
          return (_context.UserPartner?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
