using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Models
{
    [PrimaryKey(nameof(NIK))]
    public class UserPartner
    {
        [Required]
        [StringLength(16, MinimumLength = 16, ErrorMessage = "NIK must be 16 characters")]
        public required string? NIK { get; set; }

        [Required]
        [StringLength(64)]
        public string? Name { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telephone Number")]
        public required string TelephoneNumber { get; set; }

        [Required]
        [EnumDataType(typeof(Gender), ErrorMessage = "Gender is not correct.")]
        public Gender Gender { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Birth Date")]
        public DateTime BirthDate { get; set; }

        [Required]
        [DataType(DataType.Text)]
        public string? Address { get; set; }

        [Required]
        [EnumDataType(typeof(Nationality), ErrorMessage = "Nationality is not correct.")]
        public Nationality Nationality { get; set; }

        [Required]
        [EnumDataType(typeof(Citizenship), ErrorMessage = "Nationality is not correct.")]
        public Citizenship Citizenship { get; set; }
    }


    public enum Gender : byte
    {
        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "Other")]
        Other = 4
    }

    public enum Nationality : ushort
    {
        Indonesia,
        Malaysia,
        Singapore,
        India,
    }

    public enum Citizenship : ushort
    {
        Indonesian,
        Malaysian,
        Singaporean,
        Indian,
    }
}
