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
using Book_Lending_System.FileUploadService;
using Microsoft.Extensions.Logging;

namespace Book_Lending_System.Pages.BookViews.ManageView
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
        public Book Book { get; set; } = default!;

        [BindProperty]
        private string ImageData { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(uint? id)
        {
            if (id == null || _context.Book == null)
            {
                return NotFound();
            }

            var book =  await _context.Book.FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            Book = book;

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

            _context.Attach(Book).State = EntityState.Modified;

            if (Book.ImageFile != null)
            {
                byte[] bytes;
                string[] acceptedContentType = { "image/png", "image/jpeg" };
                bool contentTypeAccepted = false;
                bool error = false;
                long fileSize = Book.ImageFile.Length;
                if (fileSize > 1024 * 100)
                {
                    ModelState.AddModelError("", "Max photo size accepted: 100kB");
                    error = true;
                }

                foreach (var act in acceptedContentType)
                {
                    if (Book.ImageFile.ContentType.ToLower() == act)
                    {
                        contentTypeAccepted = true;
                        break;
                    }

                }

                if (!contentTypeAccepted)
                {
                    ModelState.AddModelError("", "Accepted file: PNG / JPG / JPEG");
                    error = true;
                }

                if (error)
                {
                    return Page();
                }

                using (Stream fs = Book.ImageFile.OpenReadStream())
                {
                    using (BinaryReader br = new(fs))
                    {
                        bytes = br.ReadBytes((Int32)fs.Length);
                    }
                }

                Book.ImageData = Convert.ToBase64String(bytes, 0, bytes.Length);
            }
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(Book.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Details", new { id = Book.Id });
        }

        private bool BookExists(uint id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
