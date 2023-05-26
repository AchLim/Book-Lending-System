#pragma warning disable CS8618
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
    public class IndexModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public IndexModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        public IList<Book> Book { get; set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Book != null && _context.BookAuthors != null && _context.BookCategories != null && _context.BookPublishers != null)
            {
                Book = await _context.Book
                                .Include(b => b.BookAuthors)!
                                    .ThenInclude(ba => ba.Author)
                                .Include(b => b.BookCategories)!
                                    .ThenInclude(bc => bc.Category)
                                .Include(b => b.BookPublishers)!
                                    .ThenInclude(bp => bp.Publisher)
                                .ToListAsync();
            }
            else if (_context.Book != null)
            {
                Book = await _context.Book.ToListAsync();
            }
        }
    }
}
