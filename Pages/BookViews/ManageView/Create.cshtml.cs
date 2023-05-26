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
using Microsoft.EntityFrameworkCore;

namespace Book_Lending_System.Pages.BookViews.ManageView
{
    public class CreateModel : PageModel
    {
        private readonly Book_Lending_System.Data.Book_Lending_SystemContext _context;
        private readonly IFileUploadService _fileUploadService;

        public CreateModel(Book_Lending_System.Data.Book_Lending_SystemContext context, IFileUploadService fileUploadService)
        {
            _context = context;
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

        public async Task<IActionResult> OnGetAsync()
        {
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
            return Page();
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid || _context.Book == null || Book == null)
            {
                return Page();
            }

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

            _context.Book.Add(Book);
            await _context.SaveChangesAsync();

            await ManageSelectedAuthors();
            await ManageSelectedCategories();
            await ManageSelectedPublishers();

            return RedirectToPage("./Details", new { id = Book.Id });
        }

        private async Task ManageSelectedAuthors()
        {
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
                await _context.SaveChangesAsync();
            }
        }

        private async Task ManageSelectedCategories()
        {
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
                await _context.SaveChangesAsync();
            }
        }

        private async Task ManageSelectedPublishers()
        {
            bool publisherDefined = (SelectedPublishers != null) && (SelectedPublishers.Count > 0);
            if (publisherDefined)
            {
                List<BookPublisher> bookPublishers = new(SelectedPublishers!.Count);

                foreach (var publisherId in SelectedPublishers)
                {
                    BookPublisher bp = new() { BookId = Book.Id, PublisherId = publisherId };
                    bookPublishers.Add(bp);
                }

                _context.BookPublishers.AddRange(bookPublishers);
                await _context.SaveChangesAsync();
            }
        }
    }
}
