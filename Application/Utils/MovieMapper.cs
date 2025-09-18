using Domain.Interface.Services.Movie;
using Domain.Models.Contents;



namespace Application.Utils
{
    public class MovieMapper : IMovieMapper
    {
        public Content ToEntity(FavoriteMovieDTO movie)
        {
            if (movie == null)
            {
                return null;
            }

            return new Content
            {
                Id = movie.id,
                Title = movie.Title,
                ImageUrl = movie.ImageUrl,
                User = null
            };
        }

        public IEnumerable<Content> ToEntities(IEnumerable<FavoriteMovieDTO> movies)
        {
            return movies?.Select(ToEntity) ?? Enumerable.Empty<Content>();
        }
    }
}
