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
                AlreadySeen = movie.IsAlreadySeen,
                FavoritedByUsers = null
            };
        }
        public FavoriteContentDTO ToDTO(Content content)
        {
            return new FavoriteContentDTO(content.Id, content.AlreadySeen);
        }

        public IEnumerable<Content> ToEntities(IEnumerable<FavoriteContentDTO> movies)
        {
            return movies?.Select(ToEntity) ?? Enumerable.Empty<Content>();
        }
    }
}
