using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;

namespace Book_Lending_System.Pages.Students
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly ILogger<EditModel> _logger;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context, ILogger<EditModel> logger)
        {
            _context = context;
            _logger = logger;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        public SelectList UserAccountSelectList = default!;

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student =  await _context.Student.FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }
            Student = student;
            this.UserAccountSelectList = new(GetUserAccounts());
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
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

            _context.Attach(Student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(Student.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool StudentExists(uint id)
        {
          return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public IEnumerable<string> GetUserAccounts()
        {
            DbSet<UserAccount> userAccounts = _context.UserAccount;
            List<string> userAccountName = new();
            foreach (UserAccount acc in userAccounts)
            {
                userAccountName.Add(acc.Username);
            }
            return userAccountName.AsEnumerable();
        }

        public SelectList GetUserAccountSelectList()
        {
            DbSet<UserAccount> UserAccount = _context.UserAccount;
            DbSet<Student> Student = _context.Student;

            IQueryable<uint> nonavailableUserAccountIds = from s in Student where s.UserAccountId != null select s.Id;
            IQueryable<uint> userAccountIds = from u in UserAccount select u.Id;

            IQueryable<uint> availableUserAccountIds = userAccountIds.Except(nonavailableUserAccountIds);

            List<UserAccount> availableUserAccounts = (from u in UserAccount where availableUserAccountIds.Contains(u.Id) select u).ToList();

            if (this.Student.UserAccountId.HasValue)
            {
                UserAccount? currentUserAccount = (from u in UserAccount where u.Id == this.Student.UserAccountId orderby u.Id ascending select u).SingleOrDefault();
                if (currentUserAccount != default)
                {
                    availableUserAccounts.Add(currentUserAccount);
                }
            }

            return new SelectList(availableUserAccounts, "Id", "Username");
        }  
    }
}
