using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Book_Lending_System.Data;
using Book_Lending_System.Models;
using Book_Lending_System.FileUploadService;

namespace Book_Lending_System.Pages.BookViews.ManageView
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
        public Book Book { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Book == null || Book == null)
            {
                return Page();
            }

            byte[] bytes;
            if (Book.ImageFile != null)
            {
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

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Details", new { id = Book.Id });
        }
    }
}
