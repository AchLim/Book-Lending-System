using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Book_Lending_System.Data
{
    public class Book_Lending_SystemContext : DbContext
    {
        public Book_Lending_SystemContext (DbContextOptions<Book_Lending_SystemContext> options)
            : base(options)
        {
        }

        public DbSet<Book_Lending_System.Models.Book> Book { get; set; } = default!;

        public DbSet<Book_Lending_System.Models.Student> Student { get; set; } = default!;

        public DbSet<Book_Lending_System.Models.UserAccount> UserAccount { get; set; } = default!;

        public DbSet<Book_Lending_System.Models.Role> Role { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>()
                .HasOne(_ => _.UserAccount)
                .WithMany(_ => _.Roles)
                .HasForeignKey(_ => _.UserAccountId);

            modelBuilder.Entity<Student>()
                .HasOne(_ => _.UserAccount)
                .WithOne()
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
