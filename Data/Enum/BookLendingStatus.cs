using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Data
{
    public enum BookLendingStatus : byte
    {
        [Display(Name = "Submitted")]
        Submitted = 5,

        [Display(Name = "Approved")]
        Approved = 10,

        [Display(Name = "Pending Payment Due")]
        Pending_Payment_Due = 15,

        [Display(Name = "Returned")]
        Returned = 20,

        [Display(Name = "Rejected")]
        Rejected = 25,

        [Display(Name = "Cancelled")]
        Cancelled = 30,
    }
}