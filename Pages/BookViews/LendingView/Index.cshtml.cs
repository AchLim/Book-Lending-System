using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.BookViews.LendingView
{
    public class IndexModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly AccessRight _accessRight;

        public IndexModel(Book_Lending_System.Data.Book_Lending_SystemContext context, AccessRight accessRight)
        {
            _context = context;
            _accessRight = accessRight;
        }

        public IList<UserBook> UserBook { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.UserBook != null)
            {
                IdentityUser? currentUser = await _accessRight.GetCurrentUser(User);
                UserPartner? userPartner = null;
                if (currentUser != null && _context.UserPartner != null)
                {
                    userPartner = _context.UserPartner.FirstOrDefault(up => up.User.Id == currentUser.Id);
                }
                if (await _accessRight.IsStaff(User) || await _accessRight.IsAdmin(User))
                {
                    UserBook = await _context.UserBook
                    .Include(u => u.Book)
                    .Include(u => u.User).ToListAsync();
                }

                if (userPartner != null)
                {
                    UserBook = await _context.UserBook
                                            .Include(u => u.Book)
                                            .Include(u => u.User)
                                            .Where(u => u.UserId == userPartner.Id)
                                            .ToListAsync();
                }
            }
            return Page();
        }
    }
}
