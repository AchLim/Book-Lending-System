using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Book_Lending_System.Data
{
    public class Book_Lending_SystemContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        public Book_Lending_SystemContext (DbContextOptions<Book_Lending_SystemContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<UserPartner> UserPartner { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserBook>(userBook =>
            {
                userBook.HasKey(ub => new { ub.UserId, ub.BookId });

                userBook.HasOne(ub => ub.Book)
                        .WithMany(b => b.UserBooks)
                        .HasForeignKey(ub => ub.BookId)
                        .IsRequired();

                userBook.HasOne(ub => ub.User)
                        .WithMany(u => u.UserBooks)
                        .HasForeignKey(ub => ub.UserId)
                        .IsRequired();
            });

            modelBuilder.Entity<Category>()
                        .HasIndex(c => c.Name)
                        .IsUnique();

            modelBuilder.Entity<BookCategory>(bookCategory =>
            {
                bookCategory.HasKey(bc => new { bc.BookId, bc.CategoryId });

                bookCategory.HasOne(bc => bc.Book)
                        .WithMany(b => b.BookCategories)
                        .HasForeignKey(bc => bc.BookId)
                        .IsRequired();

                bookCategory.HasOne(bc => bc.Category)
                        .WithMany(u => u.BookCategories)
                        .HasForeignKey(bc => bc.CategoryId)
                        .IsRequired();
            });
        }

        public DbSet<Book_Lending_System.Models.Category> Category { get; set; } = default!;
    }
}
