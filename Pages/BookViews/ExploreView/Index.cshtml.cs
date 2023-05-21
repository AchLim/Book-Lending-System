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

namespace Book_Lending_System.Pages.BookViews.ExploreView
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public IndexModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

        public IList<Book> TopRatedBooks { get; set; } = default!;
        public IList<Book> Book { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Book != null)
            {
                TopRatedBooks = await _context.Book.Take(5).ToListAsync();
                Book = await _context.Book.ToListAsync();
            }
        }
    }
}
