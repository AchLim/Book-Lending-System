using System;
namespace Book_Lending_System.Models
{
    /// <summary>
    /// The default implementation of <see cref="BookPublisher{TBookKey, TAuthorKey}"/> which uses a string as a book primary key, and string as a publisher primary key.
    /// </summary>
    public class BookPublisher : BookPublisher<string, string> { }
    ///
    /// <summary>
    /// Represents the link between a book and a publisher.
    /// </summary>
    /// <typeparam name="TBookKey">The type of the primary key used for book.</typeparam>
    /// <typeparam name="TPublisherKey">The type of the primary key used for publisher.</typeparam>
    public class BookPublisher<TBookKey, TPublisherKey> where TBookKey : IEquatable<TBookKey> where TPublisherKey : IEquatable<TPublisherKey>
    {
        public Book Book { get; set; } = default!;
        public TBookKey BookId { get; set; } = default!;

        public Publisher Publisher { get; set; } = default!;
        public TPublisherKey PublisherId { get; set; } = default!;
    }
}