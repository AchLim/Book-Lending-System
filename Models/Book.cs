using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    public class Book
    {
        public Book()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Display(Name = "Authors")]
        public ICollection<BookAuthor>? BookAuthors { get; set; }

        [Display(Name = "Publishers")]
        public ICollection<BookPublisher>? BookPublishers { get; set; }

        [Required]
        [DataType(DataType.MultilineText)]
        public required string Synopsis { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [EnumDataType(typeof(BookStatus))]
        [Display(Name = "Status")]
        public BookStatus Status { get; set; }

        [Display(Name = "Image")]
        public string? ImageLocation { get; set; }

        [NotMapped]
        [Display(Name = "Image File")]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Categories")]
        public ICollection<BookCategory>? BookCategories { get; set; }
    }
}