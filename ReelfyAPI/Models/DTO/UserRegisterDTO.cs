using System.ComponentModel.DataAnnotations;

namespace ReelfyAPI.Models.DTO
{
    public record UserRegisterDTO(
    [Required]
    [EmailAddress]
    string email,

    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
    string password,

    [Required]
    int age,

    [Required]
    string phoneNumber)
    { }
        
}
