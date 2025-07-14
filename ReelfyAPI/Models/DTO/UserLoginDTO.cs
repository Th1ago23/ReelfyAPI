using System.ComponentModel.DataAnnotations;

namespace ReelfyAPI.Models.DTO
{
    public class UserLoginDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string Password { get; set; }
    }
}
