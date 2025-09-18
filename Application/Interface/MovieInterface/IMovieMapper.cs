using Application.DTO.Content;
using Domain.Models.Contents;


namespace Domain.Interface.Services.Movie;

public interface IMovieMapper
{
    //Entity
    public Content ToEntity(FavoriteContentDTO favoriteMovieDTO);
    public IEnumerable<Content> ToEntities(IEnumerable<FavoriteContentDTO> favoriteMovies);

}
