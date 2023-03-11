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
    public class Book_Lending_SystemContext : IdentityDbContext<Account, Role, string, IdentityUserClaim<string>,
                                                AccountRole, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public Book_Lending_SystemContext (DbContextOptions<Book_Lending_SystemContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Account { get; set; } = default!;
        public DbSet<Book> Book { get; set; } = default!;
        public DbSet<Role> Role { get; set; } = default!;
        public DbSet<Student> Student { get; set; } = default!;
        public DbSet<UserPartner> UserPartner { get; set; } = default!;
        public DbSet<AccountRole> AccountRole { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<AccountRole>(accountRole =>
            {
                accountRole.HasKey(ar => new { ar.UserId, ar.RoleId });

                accountRole.HasOne(ar => ar.Role)
                            .WithMany(r => r.AccountRoles)
                            .HasForeignKey(ar => ar.RoleId)
                            .IsRequired();

                accountRole.HasOne(ar => ar.Account)
                            .WithMany(a => a.AccountRoles)
                            .HasForeignKey(ar => ar.UserId)
                            .IsRequired();
            });
        }
    }
}
