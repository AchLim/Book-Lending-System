using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Pages.StudentView
{
    public class CreateModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public CreateModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ICollection<UserPartner> UserPartners { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.UserPartner != null && _context.Student != null)
            {
                var userPartners = await _context.UserPartner.ToListAsync();
                var students = await _context.Student.Include(s => s.UserPartner).ToListAsync();

                UserPartners = new List<UserPartner>();

                foreach (var userPartner in userPartners)
                    UserPartners.Add(userPartner);

                foreach (var student in students)
                {
                    if (student.UserPartner != null)
                    {
                        UserPartners.Remove(student.UserPartner);
                    }
                }

                //UserPartners = await _context.UserPartner.ToListAsync();
            }

            return Page();
        }

        [BindProperty]
        public Student Student { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Student == null || Student == null)
            {
                return await OnGetAsync();
            }

            Student? duplicatedNPM = _context.Student.Where(student => student.NPM == Student.NPM).FirstOrDefault();
            if (duplicatedNPM != null)
            {
                ModelState.AddModelError("DuplicateNPM", "Duplicated NPM found! Please contact IT Department!");
                return await OnGetAsync();
            }

            _context.Student.Add(Student);
            await _context.SaveChangesAsync();


            return RedirectToPage("./Details", new { id = Student.Id });
        }
    }
}
