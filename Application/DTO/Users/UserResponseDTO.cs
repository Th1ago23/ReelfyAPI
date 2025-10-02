namespace Application.DTO.Users
{
    public record UserResponseDTO(int Id, string name, string Email, DateTime? CreatedAt)
    {    }
}
