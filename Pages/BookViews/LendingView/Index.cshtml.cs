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

        public IList<LendRequest> LendRequest { get;set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.LendRequest != null)
            {
                // Allow staff / admin to see all requests.
                if (await _accessRight.IsStaff(User) || await _accessRight.IsAdmin(User))
                {
                    LendRequest = await _context.LendRequest
                    .Include(u => u.Book)
                    .Include(u => u.User).ToListAsync();

                    return Page();
                }
                IdentityUser? currentUser = await _accessRight.GetCurrentUser(User);
                if (currentUser == null || _context.UserPartner == null)
                    return Page();

                var userPartner = _context.UserPartner.FirstOrDefault(up => up.User.Id == currentUser.Id);
                if (userPartner == null)
                    return Page();

                // Self requests.
                LendRequest = await _context.LendRequest
                                        .Include(u => u.Book)
                                        .Include(u => u.User)
                                        .Where(u => u.UserId == userPartner.Id)
                                        .ToListAsync();
            }
            return Page();
        }
    }
}
