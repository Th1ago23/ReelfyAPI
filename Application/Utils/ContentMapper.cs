using Application.DTO.Content;
using Application.Interface.ContentInterface;

using Domain.Models.Contents;



namespace Application.Utils
{
    public class ContentMapper : IContentMapper
    {
        public Content ToEntity(FavoriteContentDTO movie)
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

        public IEnumerable<Content> ToEntities(IEnumerable<FavoriteContentDTO> movies)
        {
            return movies?.Select(ToEntity) ?? Enumerable.Empty<Content>();
        }
    }
}
