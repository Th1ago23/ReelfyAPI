using Application.DTO.Content;
using Domain.Models.Contents;

namespace Application.Interface.Mappers;

public interface IGenreMapper
{
    public Genre ToEntity(GenreAddDTO dto);
    public IEnumerable<Genre> ToEntities (IEnumerable<GenreAddDTO> dtos);
}
