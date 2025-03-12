using System.ComponentModel.DataAnnotations;

namespace Contracts.User
{
    public class GoogleSignInDto
    {
        [Required]
        public required string IdToken { get; set; }
    }
}
