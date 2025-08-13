using Domain.Interface.Services.Movie;
using Domain.Models;
using Domain.Models.DTO;



namespace Application.Utils
{
    public class MovieMapper : IMovieMapper
    {
        public FavoriteMovie ToEntity(FavoriteMovieDTO movie)
        {
            if (movie == null)
            {
                return null;
            }

            return new FavoriteMovie
            {
                Id = movie.id,
                Title = movie.Title,
                ImageUrl = movie.ImageUrl,
                User = null
            };
        }

        public IEnumerable<FavoriteMovie> ToEntities(IEnumerable<FavoriteMovieDTO> movies)
        {
            return movies?.Select(ToEntity) ?? Enumerable.Empty<FavoriteMovie>();
        }
    }
}
