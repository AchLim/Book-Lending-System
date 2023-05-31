using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Authorization;
using Book_Lending_System.FileUploadService;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.BookViews.ExploreView
{
    [AllowAnonymous]
    public class DetailsModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly AccessRight _accessRight;

        public DetailsModel(Book_Lending_System.Data.Book_Lending_SystemContext context, AccessRight accessRight)
        {
            _context = context;
            _accessRight = accessRight;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public LendRequest LendRequest { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            Book? book = null;

            if (_context.Book != null && _context.BookAuthors != null && _context.BookCategories != null && _context.BookPublishers != null)
            {
                book = await _context.Book
                                .Include(b => b.BookAuthors)!
                                    .ThenInclude(ba => ba.Author)
                                .Include(b => b.BookCategories)!
                                    .ThenInclude(bc => bc.Category)
                                .Include(b => b.BookPublishers)!
                                    .ThenInclude(bp => bp.Publisher)
                            .FirstOrDefaultAsync(m => m.Id == id);
            }
            else if (_context.Book != null)
            {
                book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);
            }

            if (book == null)
            {
                return NotFound();
            }
            Book = book;

            UserPartner? user = await GetUserPartnerFromSelf();
            if (user == null)
            {
                ModelState.AddModelError("", "Registration is required to borrow book, please contact staff.");
                return Page();
            }

            LendRequest = new()
            {
                BookId = book.Id,
                UserId = user.Id,
            };

            return Page();
        }
        
        public async Task<UserPartner?> GetUserPartnerFromSelf()
        {
            if (_context.UserPartner == null)
                return null;

            IdentityUser? currentUser = await _accessRight.GetCurrentUser(User);
            if (currentUser == null)
                return null;

            UserPartner? user = await _context.UserPartner.FirstOrDefaultAsync(up => up.User.Id == currentUser.Id);
            if (user == null)
                return null;

            return user;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (_context.LendRequest == null || Book == null)
            {
                return NotFound();
            }

            if ((await GetUserPartnerFromSelf()) == null)
                return await OnGetAsync(Book.Id);

            UserPartner? user = await GetUserPartnerFromSelf();
            if (user == null)
            {
                ModelState.AddModelError("", "User record is not found, please contact IT Department.");
                return await OnGetAsync(Book.Id);
            }

            LendRequest lendRequestToCreate = new()
            {
                BookId = Book.Id,
                UserId = user.Id,
                DateRequested = DateTime.UtcNow.Date,
                Status = BookLendingStatus.Submitted
            };

            _context.LendRequest.Add(lendRequestToCreate);
            await _context.SaveChangesAsync();

            return RedirectToPage("../LendingView/Details", new { id = lendRequestToCreate.Id });
        }   
    }
}
