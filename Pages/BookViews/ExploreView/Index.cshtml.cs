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

        public List<Category> Category { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            if (_context.BookCategories != null)
            {
                Category = await _context.Category.ToListAsync();
            }

            return Page();
        }

        public async Task<IEnumerable<Book>> GetAllBookForCategory(Category category)
        {
            if (_context.BookCategories != null)
            {
                var books = await _context.BookCategories.Where(bc => bc.CategoryId == category.Id).Select(bc => bc.Book).ToListAsync();
                return books;
            }

            return Enumerable.Empty<Book>();
        }
    }
}
