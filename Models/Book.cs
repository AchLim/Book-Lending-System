using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    [PrimaryKey(nameof(Id))]
    public class Book
    {
        public uint Id { get; set; }

        [Required]
        [StringLength(200)]
        public required string Title { get; set; }

        [Required]
        public required string Author { get; set; }

        [Required]
        public required string Synopsis { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [EnumDataType(typeof(BookStatus))]
        [Display(Name = "Book Status")]
        public BookStatus Status { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Image")]
        public string? ImageData { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
    }
}