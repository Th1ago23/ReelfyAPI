using Application.DTO.Content;
using Domain.Models.Contents;


namespace Application.Interface.ContentInterface;

public interface IContentMapper
{
    public FavoriteContentDTO ToDTO(Content content);
    public Content ToEntity(FavoriteContentDTO favoriteMovieDTO);
    public IEnumerable<Content> ToEntities(IEnumerable<FavoriteContentDTO> favoriteMovies);

}
