﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    /// <summary>
    /// The default implementation of <see cref="BookCategory{TBookKey, TCategoryKey}"/> which uses a string as a book primary key, and string as a category primary key.
    /// </summary>
    public class BookCategory : BookCategory<string, string> { }

    /// <summary>
    /// Represents the link between a user and a category.
    /// </summary>
    /// <typeparam name="TBookKey">The type of the primary key used for book.</typeparam>
    /// <typeparam name="TCategoryKey">The type of the primary key used for category.</typeparam>
    public class BookCategory<TBookKey, TCategoryKey> where TBookKey : IEquatable<TBookKey> where TCategoryKey : IEquatable<TCategoryKey>
    {
        public virtual TBookKey BookId { get; set; } = default!;

        [ForeignKey(nameof(BookId))]
        public virtual Book Book { get; set; } = default!;

        public virtual TCategoryKey CategoryId { get; set; } = default!;

        [ForeignKey(nameof(CategoryId))]
        public virtual Category Category { get; set; } = default!;
    }
}