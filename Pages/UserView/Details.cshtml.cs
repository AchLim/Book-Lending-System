using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;

namespace Book_Lending_System.Pages.UserView
{
    public class DetailsModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public DetailsModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        public UserPartner UserPartner { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.UserPartner == null)
            {
                return NotFound();
            }

            var userpartner = await _context.UserPartner.Include(up => up.User).FirstOrDefaultAsync(m => m.Id == id);
            if (userpartner == null)
            {
                return NotFound();
            }
            else 
            {
                UserPartner = userpartner;
            }
            return Page();
        }
    }
}
