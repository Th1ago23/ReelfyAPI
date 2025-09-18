using Application.DTO.Content;

namespace Application.Interface.MovieInterface
{
    public interface IMovieService
    {
        Task<FavoriteContentDTO> Favorite(FavoriteContentDTO movie);
        Task<bool> RemoveFavorite(int id);

    }
}
