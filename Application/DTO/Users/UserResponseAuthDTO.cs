using Application.DTO.Content.Preferences;

namespace Application.DTO.Users
{
    public record UserResponseAuthDTO(int Id, string name, string Email, bool userIsPremium, string token, PreferenceResponseDTO PreferenceResponseDTO)
    {
    }
}
