using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using System.Drawing;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Book_Lending_System.Pages.BookViews.LendingView
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly AccessRight _accessRight;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context, AccessRight accessRight)
        {
            _context = context;
            _accessRight = accessRight;
        }

        [BindProperty]
        public LendRequest LendRequest { get; set; } = default!;

        [BindProperty]
        public ICollection<Book> Books { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (!(await _accessRight.IsAdmin(User)))
                return Unauthorized();

            if (id == null || _context.LendRequest == null)
            {
                return NotFound();
            }

            var userBook = await _context.LendRequest
                                    .Include(ub => ub.Book)
                                    .Include(ub => ub.User)
                                    .FirstOrDefaultAsync(ub => ub.Id == id);
            if (userBook == null)
            {
                return NotFound();
            }
            else 
            {
                LendRequest = userBook;
            }

            if (_context.Book != null)
            {
                Books = await _context.Book.ToListAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            switch (LendRequest.Status)
            {
                case BookLendingStatus.Rejected:
                    {
                        if (String.IsNullOrWhiteSpace(LendRequest.RejectionReason))
                        {
                            ModelState.AddModelError("", "Reason of Rejection cannot be empty if status is set to Rejected.");
                            return await OnGetAsync(LendRequest.Id);
                        }
                    }
                    break;
                case BookLendingStatus.Returned:
                    {
                        if (LendRequest.DateReturned == null)
                        {
                            ModelState.AddModelError("", "Returned Date cannot be empty if status is set to Returned.");
                            return await OnGetAsync(LendRequest.Id);
                        }
                    }
                    break;
                case 0:
                    {
                        ModelState.AddModelError("", "Status cannot be empty.");
                        return await OnGetAsync(LendRequest.Id);
                    }

                default:
                    break;

            }

            if (LendRequest.EndDate < LendRequest.StartDate)
            {
                ModelState.AddModelError("", "End Date cannot be earlier than Start Date.");
                return await OnGetAsync(LendRequest.Id);
            }

            if (LendRequest.DateReturned < LendRequest.StartDate)
            {
                ModelState.AddModelError("", "Return Date cannot be earlier than Start Date.");
                return await OnGetAsync(LendRequest.Id);
            }

            if (_context.LendRequest == null)
                return NotFound();

            LendRequest? requestToUpdate = await _context.LendRequest.FirstOrDefaultAsync(lr => lr.Id == LendRequest.Id);
            if (requestToUpdate == null)
                return NotFound();

            requestToUpdate.BookId = LendRequest.BookId;
            requestToUpdate.Status = LendRequest.Status;
            requestToUpdate.RejectionReason = LendRequest.RejectionReason;
            requestToUpdate.DateRequested = LendRequest.DateRequested;
            requestToUpdate.StartDate = LendRequest.StartDate;
            requestToUpdate.EndDate = LendRequest.EndDate;
            requestToUpdate.DateReturned = LendRequest.DateReturned;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LendRequestExists(LendRequest.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = LendRequest.Id });
        }

        private bool LendRequestExists(string id)
        {
            return (_context.LendRequest?.Any(lr => lr.Id == id)).GetValueOrDefault();
        }

    }
}
