using System;
namespace Book_Lending_System.Models
{
    /// <summary>
    /// The default implementation of <see cref="BookAuthor{TBookKey, TAuthorKey}"/> which uses a string as a book primary key, and string as a author primary key.
    /// </summary>
    public class BookAuthor : BookAuthor<string, string> { }
    ///
    /// <summary>
    /// Represents the link between a book and a author.
    /// </summary>
    /// <typeparam name="TBookKey">The type of the primary key used for book.</typeparam>
    /// <typeparam name="TAuthorKey">The type of the primary key used for author.</typeparam>
    public class BookAuthor<TBookKey, TAuthorKey> where TBookKey : IEquatable<TBookKey> where TAuthorKey : IEquatable<TAuthorKey>
    {
        public Book Book { get; set; } = default!;
        public TBookKey BookId { get; set; } = default!;

        public Author Author { get; set; } = default!;
        public TAuthorKey AuthorId { get; set; } = default!;
    }
}