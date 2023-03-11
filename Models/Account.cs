using Book_Lending_System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace Book_Lending_System.Models
{
    public class Account : IdentityUser<string>
    {
        public Account()
        {
            Id = Guid.NewGuid().ToString();
            SecurityStamp = Guid.NewGuid().ToString();
        }

        [InverseProperty("Account")]
        public virtual ICollection<AccountRole>? AccountRoles { get; set; }
    }
}
