using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;

namespace Book_Lending_System.Pages.BookViews.ManageView
{
    public class DeleteModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public DeleteModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;
        public IList<string> BookAuthors { get; set; } = default!;
        public IList<string> BookCategories { get; set; } = default!;
        public IList<string> BookPublishers { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book = await _context.Book.FirstOrDefaultAsync(m => m.Id == id);

            if (book == null)
            {
                return NotFound();
            }
            else 
            {
                Book = book;
            }

            if (_context.BookAuthors != null)
            {
                BookAuthors = await _context.BookAuthors.Include(ba => ba.Author).Where(ba => ba.BookId == book.Id).Select(ba => ba.Author).Select(a => a.Name).ToListAsync();
            }

            if (_context.BookCategories != null)
            {
                BookCategories = await _context.BookCategories.Include(bc => bc.Category).Where(bc => bc.BookId == book.Id).Select(bc => bc.Category).Select(c => c.Name).ToListAsync();
            }

            if (_context.BookPublishers != null)
            {
                BookPublishers = await _context.BookPublishers.Include(bp => bp.Publisher).Where(bp => bp.BookId == book.Id).Select(bp => bp.Publisher).Select(p => p.Name).ToListAsync();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }
            var book = await _context.Book.FindAsync(id);

            if (book != null)
            {
                Book = book;
                _context.Book.Remove(Book);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
