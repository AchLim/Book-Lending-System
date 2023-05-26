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
        private readonly IFileUploadService _fileUploadService;

        public EditModel(Book_Lending_System.Data.Book_Lending_SystemContext context, ILogger<EditModel> logger, IFileUploadService fileUploadService)
        {
            _context = context;
            _logger = logger;
            _fileUploadService = fileUploadService;
        }

        [BindProperty]
        public Book Book { get; set; } = default!;

        [BindProperty]
        public List<Author> Author { get; set; } = default!;

        [BindProperty]
        public ICollection<string> SelectedAuthors { get; set; }

        [BindProperty]
        public List<Category> Categories { get; set; } = default!;

        [BindProperty]
        public ICollection<string> SelectedCategories { get; set; }

        [BindProperty]
        public List<Publisher> Publisher { get; set; } = default!;

        [BindProperty]
        public ICollection<string> SelectedPublishers { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
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

            if (_context.Author != null)
            {
                Author = await _context.Author.ToListAsync();
            }
            if (_context.Category != null)
            {
                Categories = await _context.Category.ToListAsync();
            }
            if (_context.Publisher != null)
            {
                Publisher = await _context.Publisher.ToListAsync();
            }

            if (_context.BookAuthors != null)
            {
                SelectedAuthors = await _context.BookAuthors.Where(ba => ba.BookId == book.Id).Select(ba => ba.AuthorId).ToListAsync();
            }

            if (_context.BookCategories != null)
            {
                SelectedCategories = await _context.BookCategories.Where(bc => bc.BookId == book.Id).Select(bc => bc.CategoryId).ToListAsync();
            }

            if (_context.BookPublishers != null)
            {
                SelectedPublishers = await _context.BookPublishers.Where(bp => bp.BookId == book.Id).Select(bp => bp.PublisherId).ToListAsync();
            }


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
                string[] acceptedContentType = { "image/png", "image/jpeg" };
                bool contentTypeAccepted = false;
                bool error = false;
                long fileSize = Book.ImageFile.Length;
                int maxFileSize = 2097152; // 2 MB
                if (fileSize > maxFileSize)
                {
                    ModelState.AddModelError("", "The file is too large, consider upload file with less than 2 MB");
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

                if (_fileUploadService != null)
                {
                    string createdFilePath = await _fileUploadService.UploadFileAsync(Book.ImageFile, Book.Id);
                    Book.ImageLocation = createdFilePath;
                }
            }

            await ManageAuthorMany2Many();
            await ManageCategoryMany2Many();
            await ManagePublisherMany2Many();

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

        private bool BookExists(string id)
        {
          return (_context.Book?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private async Task ManageAuthorMany2Many()
        {
            List<BookAuthor> currentBookAuthorList = await _context.BookAuthors.Where(bc => bc.BookId == Book.Id).ToListAsync();
            if (currentBookAuthorList.Count > 0)
            {
                _context.BookAuthors.RemoveRange(currentBookAuthorList);
            }

            bool authorDefined = (SelectedAuthors != null) && (SelectedAuthors.Count > 0);
            if (authorDefined)
            {
                List<BookAuthor> bookAuthors = new(SelectedAuthors!.Count);

                foreach (var authorId in SelectedAuthors)
                {
                    BookAuthor ba = new() { BookId = Book.Id, AuthorId = authorId };
                    bookAuthors.Add(ba);
                }

                _context.BookAuthors.AddRange(bookAuthors);
            }
        }

        private async Task ManageCategoryMany2Many()
        {
            List<BookCategory> currentBookCategoryList = await _context.BookCategories.Where(bc => bc.BookId == Book.Id).ToListAsync();
            if (currentBookCategoryList.Count > 0)
            {
                _context.BookCategories.RemoveRange(currentBookCategoryList);
            }

            bool categoryDefined = (SelectedCategories != null) && (SelectedCategories.Count > 0);
            if (categoryDefined)
            {
                List<BookCategory> bookCategories = new(SelectedCategories!.Count);

                foreach (var category in SelectedCategories)
                {
                    BookCategory bc = new() { BookId = Book.Id, CategoryId = category };
                    bookCategories.Add(bc);
                }

                _context.BookCategories.AddRange(bookCategories);
            }
        }

        private async Task ManagePublisherMany2Many()
        {
            List<BookPublisher> currentBookPublisherList = await _context.BookPublishers.Where(bc => bc.BookId == Book.Id).ToListAsync();
            if (currentBookPublisherList.Count > 0)
            {
                _context.BookPublishers.RemoveRange(currentBookPublisherList);
            }

            bool publisherDefiend = (SelectedPublishers != null) && (SelectedPublishers.Count > 0);
            if (publisherDefiend)
            {
                List<BookPublisher> bookPublishers = new(SelectedPublishers!.Count);

                foreach (var publisherId in SelectedPublishers)
                {
                    BookPublisher bp = new() { BookId = Book.Id, PublisherId = publisherId };
                    bookPublishers.Add(bp);
                }

                _context.BookPublishers.AddRange(bookPublishers);
            }
        }
    }
}
