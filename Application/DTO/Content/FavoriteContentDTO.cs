using Domain.Utils;

namespace Application.DTO.Content
{
    public record FavoriteContentDTO
    (int id, string Title, Category Category, string ImageUrl, bool IsAlreadySeen)
    { }
}
