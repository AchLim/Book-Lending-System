using Book_Lending_System.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Models;


/// <summary>
/// The default implementation of <see cref="UserBook{TUserKey, TBookKey}"/> which uses a string as a user primary key, and string as book primary key.
/// </summary>
public class UserBook : UserBook<string, string> { }

/// <summary>
/// Represents the link between a user and a book.
/// </summary>
/// <typeparam name="TBookKey">The type of the primary key used for book.</typeparam>
/// <typeparam name="TUserKey">The type of the primary key used for user.</typeparam>

[PrimaryKey(nameof(Id))]
public class UserBook<TUserKey, TBookKey> where TUserKey : IEquatable<TUserKey> where TBookKey : IEquatable<TBookKey>
{
    public UserBook()
    {
        Id = Guid.NewGuid().ToString();
    }

    public string Id { get; set; }
    public virtual TUserKey UserId { get; set; } = default!;
    public virtual UserPartner User { get; set; } = default!;

    public virtual TBookKey BookId { get; set; } = default!;
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
    public DateTime DateReturned { get; set; }

    [EnumDataType(typeof(BookLendingStatus))]
    [Display(Name = "Status")]
    public BookLendingStatus Status { get; set; }
}