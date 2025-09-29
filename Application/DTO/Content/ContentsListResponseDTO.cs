namespace Application.DTO.Content;

public record ContentsListResponseDTO(int userId, int contentListId, IEnumerable<FavoriteContentDTO> contentsDTO)
{
}
