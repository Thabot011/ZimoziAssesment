using System.ComponentModel.DataAnnotations;

namespace Contracts.User
{
    public class AddUserDto
    {
        [Required]
        public required string FullName { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public required string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }
        [Required]
        public UserRole UserRole { get; set; }
    }

    public enum UserRole
    {
        Admin,
        NormalUser
    }
}
