using System.ComponentModel.DataAnnotations;

namespace Application.DTO.Users
{
    public record UserLoginDTO
    (
        [Required]
        [EmailAddress]
        string Email,

        [Required]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        string Password
    )
    { }
}
