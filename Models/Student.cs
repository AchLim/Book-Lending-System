using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    public class Student
    {
        public uint Id { get; set; }

        [Required]
        [StringLength(7, ErrorMessage = "NPM must be 7 digit numbers.", MinimumLength = 7)]
        public required string NPM { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "Name must be between 4 and 40.", MinimumLength = 4)]
        public required string Name { get; set; }

        [Required]
        [Phone]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Telephone Number")]
        public required string TelephoneNumber { get; set; }

        [Required]
        [EnumDataType(typeof(StudyProgram))]
        [Display(Name = "Study Program")]
        public required StudyProgram StudyProgram { get; set; }

        [Display(Name = "User Account")]
        
        public uint? UserAccountId { get; set; }

        public UserAccount? UserAccount { get; set; }
    }
}

public enum StudyProgram : byte
{
    [Display(Name = "Civil Engineering")]
    CivilEngineering,

    [Display(Name = "Architecture")]
    Architecture,

    [Display(Name = "Electrical Engineering")]
    ElectricalEngineering,

    [Display(Name = "Information System")]
    InformationSystem,

    [Display(Name = "Information Technology")]
    InformationTechnology,

    [Display(Name = "Management")]
    Management,

    [Display(Name = "Accounting")]
    Accounting,

    [Display(Name = "Tourism")]
    Tourism,

    [Display(Name = "Master of Management")]
    MasterOfManagement,

    [Display(Name = "Law Science")]
    LawScience,

    [Display(Name = "Master of Law Study")]
    MasterOfLawStudy
        ,
    [Display(Name = "English Language Education")]
    EnglishLanguageEducation
}