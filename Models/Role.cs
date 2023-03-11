using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    public class Role : IdentityRole<string>
    {
        [InverseProperty("Role")]
        public virtual ICollection<AccountRole>? AccountRoles { get; set; }
    }
}
