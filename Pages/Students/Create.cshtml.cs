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

            DbSet<Student> students = _context.Student;
            var existingStudentNPM = (from s in students where s.NPM == Student.NPM select s.NPM).FirstOrDefault();
            if (existingStudentNPM != default)
            {
                ModelState.AddModelError("DuplicatedNPM", "NPM already exist.");
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
            DbSet<UserAccount> UserAccount = _context.UserAccount;
            DbSet<Student> Student = _context.Student;

            IQueryable<uint> nonavailableUserAccountIds = from s in Student where s.UserAccountId != null select s.Id;
            IQueryable<uint> userAccountIds = from u in UserAccount select u.Id;

            IQueryable<uint> availableUserAccountIds = userAccountIds.Except(nonavailableUserAccountIds);

            List<UserAccount> availableUserAccounts = (from u in UserAccount where availableUserAccountIds.Contains(u.Id) select u).ToList();

            return new SelectList(availableUserAccounts, "Id", "Username");
        }
    }
}
