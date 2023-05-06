
using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Data
{
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
}