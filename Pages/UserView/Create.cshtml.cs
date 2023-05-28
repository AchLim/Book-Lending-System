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
using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Pages.UserView
{
    public class CreateModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public CreateModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public UserPartner UserPartner { get; set; } = default!;

        [BindProperty]
        public ICollection<IdentityUser> IdentityUsers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.Users != null)
            {
                IdentityUsers = await _context.Users.ToListAsync();
            }

            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.UserPartner == null || UserPartner == null)
            {
                return Page();
            }

            if (UserPartnerExists(UserPartner.NIK!))
            {
                ModelState.AddModelError(string.Empty, "NIK exists!");
                return Page();
            }

            _context.UserPartner.Add(UserPartner);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        private bool UserPartnerExists(string id)
        {
            return (_context.UserPartner?.Any(e => e.NIK == id)).GetValueOrDefault();
        }
    }
}
