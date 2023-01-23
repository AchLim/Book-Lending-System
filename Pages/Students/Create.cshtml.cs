using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Pages.Students
{
    public class CreateModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public CreateModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Student == null || Student == null)
            {
                return Page();
            }

            _context.Student.Add(Student);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

        //public List<SelectListItem> GetUserAccountSelectListItem()
        //{
        //    List<SelectListItem> userAccountSelectListItem = new();
        //    DbSet<UserAccount> userAccounts = _context.UserAccount;
        //    if (userAccounts != null)
        //    {
        //        foreach (UserAccount userAccount in userAccounts)
        //        {
        //            SelectListItem s = new(userAccount.Username, userAccount.Id.ToString());
        //            userAccountSelectListItem.Add(s);
        //        }
        //    }
        //    return userAccountSelectListItem;
        //}

        public SelectList GetUserAccountSelectList()
        {
            return new SelectList(_context.UserAccount, "Id", "Username");
        }
    }
}
