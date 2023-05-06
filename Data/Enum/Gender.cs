
using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Data
{
    public enum Gender : byte
    {
        [Display(Name = "Male")]
        Male = 1,

        [Display(Name = "Female")]
        Female = 2,

        [Display(Name = "Other")]
        Other = 4
    }
}
