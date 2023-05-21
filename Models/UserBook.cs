using System.ComponentModel.DataAnnotations;

namespace Book_Lending_System.Models;


/// <summary>
/// The default implementation of <see cref="UserBook{TUserKey, TBookKey}"/> which uses a string as a user primary key, and uint as book primary key.
/// </summary>
public class UserBook : UserBook<string, uint> { }

/// <summary>
/// Represents the link between a user and a book.
/// </summary>
/// <typeparam name="TBookKey">The type of the primary key used for book.</typeparam>
/// <typeparam name="TUserKey">The type of the primary key used for user.</typeparam>
public class UserBook<TUserKey, TBookKey> where TUserKey : IEquatable<TUserKey> where TBookKey : IEquatable<TBookKey>
{
    public virtual TUserKey UserId { get; set; } = default!;
    public virtual UserPartner User { get; set; } = default!;

    public virtual TBookKey BookId { get; set; } = default!;
    public virtual Book Book { get; set; } = default!;


    [DataType(DataType.DateTime)]
    public DateTime BorrowedTime { get; set; }
}