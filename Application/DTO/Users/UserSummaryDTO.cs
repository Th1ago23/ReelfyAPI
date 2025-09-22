using Application.DTO.Content;
using Application.DTO.Content.Preferences;
using ReelfyAPI.Application.DTO;

namespace Application.DTO.Users;

public record UserSummaryDTO(int id, string name, int age, string phoneNumber, PreferenceResponseDTO preferences, IEnumerable<FavoriteContentDTO> favoriteContents, bool isPreemium)
{
    public List<LinkDTO> Links { get; set; } = new List<LinkDTO>();
}
