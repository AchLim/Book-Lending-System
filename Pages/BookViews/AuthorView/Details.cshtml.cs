﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using Book_Lending_System.Models;

namespace Book_Lending_System.Pages.BookViews.AuthorView
{
    public class DetailsModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;

        public DetailsModel(Book_Lending_System.Data.Book_Lending_SystemContext context)
        {
            _context = context;
        }

      public Author Author { get; set; } = default!; 

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null || _context.Author == null)
            {
                return NotFound();
            }

            var author = await _context.Author.FirstOrDefaultAsync(m => m.Id == id);
            if (author == null)
            {
                return NotFound();
            }
            else 
            {
                Author = author;
            }
            return Page();
        }
    }
}
