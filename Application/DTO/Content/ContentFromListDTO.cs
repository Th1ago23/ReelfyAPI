namespace Application.DTO.Content;

public record ContentFromListDTO(int userId, int listId, FavoriteContentDTO content)
{
}
