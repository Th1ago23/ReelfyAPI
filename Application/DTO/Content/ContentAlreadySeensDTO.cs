namespace Application.DTO.Content;

public record ContentAlreadySeensDTO(int userId, IEnumerable<FavoriteContentDTO> favoriteContentsDTO)
{
}
