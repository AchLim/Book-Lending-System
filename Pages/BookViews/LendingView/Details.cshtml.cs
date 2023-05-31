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
using System.ComponentModel.DataAnnotations;

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

        [BindProperty]
        public LendRequest LendRequest { get; set; } = default!;

        [BindProperty]
        public string RejectionReason { get; set; }

        [BindProperty]
        [DataType(DataType.Date)]
        public DateTime? ReturnedDate { get; set; } = DateTime.UtcNow.Date;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            // TODO: to check if current user is admin / staff. If not, only allow to show if userId == self

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
            return Page();
        }

        public async Task<IActionResult> OnPostApproveAsync(string? id)
        {
            if (!(await _accessRight.IsAdmin(User)) && !(await _accessRight.IsStaff(User)))
                return Unauthorized();

            if (id == null)
                return NotFound();

            if (_context.LendRequest != null)
            {
                LendRequest? lendRequestToUpdate = await _context.LendRequest.FirstOrDefaultAsync(ub => ub.Id == id);
                if (lendRequestToUpdate != null)
                {
                    lendRequestToUpdate.Status = BookLendingStatus.Approved;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Details", new { id = LendRequest.Id });
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostRejectAsync(string? id)
        {
            if (!(await _accessRight.IsAdmin(User)) && !(await _accessRight.IsStaff(User)))
                return Unauthorized();

            string? reason = RejectionReason;

            if (id == null)
                return NotFound();

            if (reason == null || string.IsNullOrWhiteSpace(reason))
            {
                ModelState.AddModelError("InvalidReason", "Please fill in the reason of rejection.");
                return await OnGetAsync(id);
            }

            if (_context.LendRequest != null)
            {
                LendRequest? userBookToUpdate = await _context.LendRequest.FirstOrDefaultAsync(ub => ub.Id == id);
                if (userBookToUpdate != null)
                {
                    userBookToUpdate.Status = BookLendingStatus.Rejected;
                    userBookToUpdate.RejectionReason = reason;
                    await _context.SaveChangesAsync();
                    return RedirectToPage("./Details", new { id = LendRequest.Id });
                }
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostReturnedAsync(string? id)
        {
            if (!(await _accessRight.IsAdmin(User)) && !(await _accessRight.IsStaff(User)))
                return Unauthorized();

            if (id == null)
                return NotFound();

            if (ReturnedDate == null)
            {
                ModelState.AddModelError("InvalidReason", "Please fill in the book returned date.");
                return await OnGetAsync(id);
            }

            if (_context.LendRequest != null)
            {
                // Need check if already due date, then redirect to status Pending_Payment_Due, at there will show button "Paid" which will redirect to status: Returned.
                // Else if not due date, redirect to status: Returned.

                LendRequest? lendRequestToUpdate = await _context.LendRequest.FirstOrDefaultAsync(ub => ub.Id == id);
                if (lendRequestToUpdate == null)
                    return NotFound();

                // Check if date returned is invalid (returnedDate < startDate)
                bool returnDateInvalid = lendRequestToUpdate.StartDate.CompareTo(ReturnedDate.Value) == 1;
                if (returnDateInvalid)
                {
                    ModelState.AddModelError("InvalidReason", "Please enter a valid date of book returned.");
                    return await OnGetAsync(id);
                }

                bool returnIsDueDate = lendRequestToUpdate.EndDate.CompareTo(ReturnedDate.Value) == -1;

                if (returnIsDueDate)
                    lendRequestToUpdate.Status = BookLendingStatus.Pending_Payment_Due;
                else
                    lendRequestToUpdate.Status = BookLendingStatus.Returned;

                lendRequestToUpdate.DateReturned = ReturnedDate.Value;

                await _context.SaveChangesAsync();
                return RedirectToPage("./Details", new { id = lendRequestToUpdate.Id });
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostUpdatePaidAsync(string? id)
        {
            if (!(await _accessRight.IsAdmin(User)) && !(await _accessRight.IsStaff(User)))
                return Unauthorized();

            if (id == null)
                return NotFound();

            if (_context.LendRequest != null)
            {
                LendRequest? lendRequestToUpdate = await _context.LendRequest.FirstOrDefaultAsync(ub => ub.Id == id);
                if (lendRequestToUpdate == null)
                    return NotFound();

                lendRequestToUpdate.Status = BookLendingStatus.Returned;

                await _context.SaveChangesAsync();

                return RedirectToPage("./Details", new { id = lendRequestToUpdate.Id });
            }

            return NotFound();
        }

    }
}
