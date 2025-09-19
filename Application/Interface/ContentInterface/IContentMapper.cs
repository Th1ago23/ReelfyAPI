using Application.DTO.Content;
using Domain.Models.Contents;


namespace Application.Interface.ContentInterface;

public interface IContentMapper
{
    //Entity
    public Content ToEntity(FavoriteContentDTO favoriteMovieDTO);
    public IEnumerable<Content> ToEntities(IEnumerable<FavoriteContentDTO> favoriteMovies);

}
