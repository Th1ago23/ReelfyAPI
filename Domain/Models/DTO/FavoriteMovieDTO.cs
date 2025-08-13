using Domain.Utils;

namespace Domain.Models.DTO
{
    public record FavoriteMovieDTO
    (int id, string Title, Category Category, string ImageUrl)
    { }
}
