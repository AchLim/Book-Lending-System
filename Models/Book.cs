using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

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
        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }

        [Required]
        [EnumDataType(typeof(BookStatus))]
        [Display(Name = "Book Status")]
        public BookStatus Status { get; set; }

        [Url]
        public string? ImageUrl { get; set; }
    }
}

public enum BookStatus : byte
{
    [Display(Name = "New")]
    New,

    [Display(Name = "Displayed")]
    Displayed,

    [Display(Name = "Unfit")]
    Unfit,

    [Display(Name = "Borrowed")]
    Borrowed,
}