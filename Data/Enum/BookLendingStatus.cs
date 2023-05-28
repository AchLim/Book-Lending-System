using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Data
{
    public enum BookLendingStatus : byte
    {
        [Display(Name = "Submitted")]
        Submitted,

        [Display(Name = "Approved")]
        Approved,

        [Display(Name = "Returned")]
        Returned,

        [Display(Name = "Rejected")]
        Rejected,

        [Display(Name = "Cancelled")]
        Cancelled,
    }
}