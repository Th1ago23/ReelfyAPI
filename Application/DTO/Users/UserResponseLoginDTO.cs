using Application.DTO.Content.Preferences;

namespace Application.DTO.Users
{
    public record UserResponseLoginDTO(UserResponseDTO User, string token, PreferenceResponseDTO PreferenceResponseDTO)
    {
    }
}
