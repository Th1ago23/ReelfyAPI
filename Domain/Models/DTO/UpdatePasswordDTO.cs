namespace ReelfyAPI.Models.DTO
{
    public record UpdatePasswordDTO (string Email, string CurrentPassword, string NewPassword)
    { }

}
