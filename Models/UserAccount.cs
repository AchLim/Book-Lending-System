using Book_Lending_System.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    public class UserAccount
    {
        public uint Id { get; set; }

        [StringLength(maximumLength: 16, ErrorMessage = "Value must be between 4 and 16", MinimumLength = 4)]
        public required string Username { get; set; }
        public required string Email { get; set; }

        [StringLength(maximumLength: 25, ErrorMessage = "Value must be between 8 and 25", MinimumLength = 8)]
        public required string Password { get; set; }

        public ICollection<Role>? Roles { get; set; }

        [NotMapped]
        public ICollection<RoleType>? RoleTypeList { get; set; } // Helper
    }

    public static class UserAccountHelper
    {
        public static ICollection<Role> GetRoleChanges(this UserAccount UserAccount, Book_Lending_SystemContext _context)
        {
            List<Role> roles;
            if (UserAccount.RoleTypeList == null)
                roles = new();
            else
                roles = new(UserAccount.RoleTypeList.Count);

            if (UserAccount.RoleTypeList != null && UserAccount.RoleTypeList.Count > 0 && _context.Role != null)
            {
                foreach (RoleType roleType in UserAccount.RoleTypeList)
                {
                    Role role = new()
                    {
                        UserAccountId = UserAccount.Id,
                        RoleType = roleType
                    };

                    roles.Add(role);
                }
            }
            return roles;
        }
    }
}
