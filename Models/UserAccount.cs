using System.ComponentModel.DataAnnotations;

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
    }
}
