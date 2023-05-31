
using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Data
{
    public enum BookStatus : byte
    {
        [Display(Name = "New")]
        New = 5,

        [Display(Name = "Displayed")]
        Displayed = 10,

        [Display(Name = "Unfit")]
        Unfit = 15,

        [Display(Name = "Borrowed")]
        Borrowed = 20,
    }
}