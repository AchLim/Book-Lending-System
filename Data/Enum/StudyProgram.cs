
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Data;

namespace Book_Lending_System.Data
{
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
        MasterOfLawStudy,

        [Display(Name = "English Language Education")]
        EnglishLanguageEducation
    }
}