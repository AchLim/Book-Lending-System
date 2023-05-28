using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;

namespace Book_Lending_System.Pages.BookViews.LendingView
{
    public class DetailsModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly AccessRight _accessRight;

        public DetailsModel(Book_Lending_System.Data.Book_Lending_SystemContext context, AccessRight accessRight)
        {
            _context = context;
            _accessRight = accessRight;
        }

        public UserBook UserBook { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            // TODO: to check if current user is admin / staff. If not, only allow to show if userId == self

            if (id == null || _context.UserBook == null)
            {
                return NotFound();
            }

            var userBook = await _context.UserBook
                                    .Include(ub => ub.Book)
                                    .Include(ub => ub.User)
                                    .FirstOrDefaultAsync(ub => ub.Id == id);
            if (userBook == null)
            {
                return NotFound();
            }
            else 
            {
                UserBook = userBook;
            }
            return Page();
        }
    }
}
