using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Models
{
    public class Book
    {
        public uint Id { get; set; }
        public required string Name { get; set; }
        public BookStatus Status { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Release Date")]
        public DateTime ReleaseDate { get; set; }
        public required string Author { get; set; }
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