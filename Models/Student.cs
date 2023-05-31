using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Book_Lending_System.Data;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Models
{
    public class Student
    {
        public Student ()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "NPM must be 7 digit numbers.", MinimumLength = 7)]
        public required string NPM { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Name must be between 4 and 40.", MinimumLength = 4)]
        public required string Name { get; set; }

        [Required]
        [EnumDataType(typeof(StudyProgram))]
        [Display(Name = "Study Program")]
        public required StudyProgram StudyProgram { get; set; }

        public string? UserPartnerId { get; set; }

        [DeleteBehavior(DeleteBehavior.ClientSetNull)]
        [Display(Name = "User")]
        public virtual UserPartner? UserPartner { get; set; }
    }
}