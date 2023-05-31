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
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Pages.StudentView
{
    public class EditModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Student Student { get; set; } = default!;

        [BindProperty]
        public ICollection<UserPartner> UserPartners { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string id)
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

            if (_context.UserPartner != null)
            {
                
                var userPartners = await _context.UserPartner.ToListAsync();
                var students = await _context.Student.Include(s => s.UserPartner).ToListAsync();

                UserPartners = new List<UserPartner>();

                foreach (var userPartner in userPartners)
                    UserPartners.Add(userPartner);

                foreach (var s in students)
                {
                    if (s.Id != Student.Id && s.UserPartner != null)
                    {
                        UserPartners.Remove(s.UserPartner);
                    }
                }

            }

            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(Student.Id);
            }
            _context.Attach(Student).State = EntityState.Modified;


            Student? duplicatedNPM = _context.Student.Where(student => student.NPM == Student.NPM).FirstOrDefault();
            if (duplicatedNPM != null && Student.Id != duplicatedNPM.Id)
            {
                ModelState.AddModelError("DuplicateNPM", "Duplicated NPM found! Please contact IT Department!");
                return await OnGetAsync(Student.Id);
            }

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

            return RedirectToPage("./Details", new { id = Student.Id });
        }

        private bool StudentExists(string id)
        {
          return (_context.Student?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
