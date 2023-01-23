using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    public class Role
    {
        public uint Id { get; set; }

        public uint UserAccountId { get; set; }

        public UserAccount? UserAccount { get; set; }

        public RoleType RoleType { get; set; }

        public string GetRoleNames()
        {
            return RoleType switch {
                RoleType.Admin => nameof(RoleType.Admin),
                RoleType.Staff => nameof(RoleType.Staff),
                RoleType.Student => nameof(RoleType.Student),
                _ => nameof(RoleType.Anonymous)
            };
        }
    }

    public enum RoleType : byte
    {
        [Display(Name = "Admin")]
        Admin,

        [Display(Name = "Staff")]
        Staff,

        [Display(Name = "Student")]
        Student,

        [Display(Name = "Anonymous")]
        Anonymous
    }
}
