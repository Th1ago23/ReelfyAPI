using Domain.Models.DTO;

namespace Domain.Interface.Services.Movie
{
    public interface IMovieService
    {
        Task<FavoriteMovieDTO> Favorite(FavoriteMovieDTO movie);
        Task<bool> RemoveFavorite(int id);

    }
}
