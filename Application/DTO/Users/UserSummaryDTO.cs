using Application.DTO.Content;
using Application.DTO.Content.Preferences;

namespace Application.DTO.Users;

public record UserSummaryDTO(
    int Id,
    string Name,
    int Age,
    string? PhoneNumber,
    PreferenceResponseDTO Preference,
    IEnumerable<ContentSummaryDTO> FavoriteContents,
    IEnumerable<ContentSummaryDTO> SeenContents,
    bool IsPremium);
