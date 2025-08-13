using Domain.Models;
using Domain.Models.DTO;


namespace Domain.Interface.Services.Movie
{
    public interface IMovieMapper
    {
        //Entity
        public FavoriteMovie ToEntity(FavoriteMovieDTO favoriteMovieDTO);
        public IEnumerable<FavoriteMovie> ToEntities(IEnumerable<FavoriteMovieDTO> favoriteMovies);

    }
}
