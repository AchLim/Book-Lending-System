using Book_Lending_System.Data;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Book_Lending_System.Models
{
    public class UserAccount
    {
        public uint Id { get; set; }

        [Required]
        [StringLength(maximumLength: 16, ErrorMessage = "Value must be between 4 and 16", MinimumLength = 4)]
        public required string Username { get; set; }

        [Required]
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public required string Email { get; set; }

        [Required]
        [Display(Name = "Email Confirmed")]
        public required bool EmailConfirmed { get; set; }

        [Required]
        [StringLength(maximumLength: 25, ErrorMessage = "Value must be between 8 and 25", MinimumLength = 8)]
        [DataType(DataType.Password)]
        public required string Password { get; set; }

        [NotMapped]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Password and Confirmed password do not match.")]
        [Display(Name = "Confirm Password")]
        public required string ConfirmPassword { get; set; }

        public ICollection<Role>? Roles { get; set; }

        [NotMapped]
        [Display(Name = "Role")]
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
