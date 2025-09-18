namespace Application.DTO.Users
{
    public record UpdatePasswordDTO(string Email, string CurrentPassword, string NewPassword)
    { }

}
