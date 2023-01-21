using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Book_Lending_System.Models;

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
    }
}
