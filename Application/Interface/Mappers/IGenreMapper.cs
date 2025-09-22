using Application.DTO.Content.Preferences;
using Domain.Models.Contents;

namespace Application.Interface.Mappers;

public interface IGenreMapper
{
    public Genre ToEntity(GenreAddDTO dto);
    public GenreAddDTO ToDTO(Genre genre);
    public IEnumerable<Genre> ToEntities (IEnumerable<GenreAddDTO> dtos);
}
