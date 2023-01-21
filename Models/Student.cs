using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Models
{
    public class Student
    {
        public uint Id { get; set; }

        [Range(7, 7, ErrorMessage = "NPM must be 7 digit numbers.")]
        public uint NPM { get; set; }
        public required string Name { get; set; }
        public required string TelephoneNumber { get; set; }
        public required StudyProgram StudyProgram { get; set; }

        public required uint UserAccountId { get; set; }
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