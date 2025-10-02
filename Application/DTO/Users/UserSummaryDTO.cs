using Application.DTO.Content;
using Application.DTO.Content.Preferences;

namespace Application.DTO.Users;

public record UserSummaryDTO(int id, string name, int age, string? phoneNumber, PreferenceResponseDTO preferences, IEnumerable<FavoriteContentDTO> favoriteContents, bool isPremium)
{
}
