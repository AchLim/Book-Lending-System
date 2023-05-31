using Book_Lending_System.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models;

public class LendRequest : LendRequest<string> { }

public class LendRequest<TUserKey> where TUserKey : IEquatable<TUserKey>
{
    public LendRequest()
    {
        Id = Guid.NewGuid().ToString();
        Status = BookLendingStatus.Submitted;
    }

    public string Id { get; set; }

    [Required, ForeignKey(nameof(User))]
    public virtual TUserKey UserId { get; set; } = default!;
    public virtual UserPartner User { get; set; } = default!;

    public virtual string BookId { get; set; } = default!;
    public virtual Book Book { get; set; } = default!;

    [DataType(DataType.Date)]
    [Display(Name = "Request Date")]
    public DateTime DateRequested { get; set; } = DateTime.UtcNow.Date;

    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.UtcNow.Date;

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.UtcNow.Date.AddDays(7);

    [DataType(DataType.Date)]
    [Display(Name = "Return Date")]
    public DateTime? DateReturned { get; set; }

    [EnumDataType(typeof(BookLendingStatus))]
    [Display(Name = "Status")]
    public BookLendingStatus Status { get; set; }

    [DataType(DataType.MultilineText)]
    [Display(Name = "Reason of Rejection")]
    public string? RejectionReason { get; set; }
}