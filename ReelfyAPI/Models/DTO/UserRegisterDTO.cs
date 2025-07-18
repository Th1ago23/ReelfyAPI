using System.ComponentModel.DataAnnotations;

namespace ReelfyAPI.Models.DTO
{
    public record UserRegisterDTO(
    [Required]
    [EmailAddress]
    string Email,
    [Required]
    string Name,
    [Required]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
    string Password,

    [Required]
    int Age,

    [Required]
    string PhoneNumber)
    { }
        
}
