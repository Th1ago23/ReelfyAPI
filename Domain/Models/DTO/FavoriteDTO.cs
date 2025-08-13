
namespace Domain.Models.DTO
{
    public record FavoriteDTO (int userId, string email, ICollection <FavoriteMovieDTO> movies)
    {
    }
}
