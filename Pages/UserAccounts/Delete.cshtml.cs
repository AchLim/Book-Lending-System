using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;

namespace Book_Lending_System.Pages.UserAccounts
{
    public class DeleteModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public DeleteModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
      public UserAccount UserAccount { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null || _context.UserAccount == null)
            {
                return NotFound();
            }

            var useraccount = await _context.UserAccount.FirstOrDefaultAsync(m => m.Id == id);

            if (useraccount == null)
            {
                return NotFound();
            }
            else 
            {
                UserAccount = useraccount;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(uint? id)
        {
            if (id == null || _context.UserAccount == null)
            {
                return NotFound();
            }
            var useraccount = await _context.UserAccount.FindAsync(id);

            if (useraccount != null)
            {
                UserAccount = useraccount;
                _context.UserAccount.Remove(UserAccount);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
