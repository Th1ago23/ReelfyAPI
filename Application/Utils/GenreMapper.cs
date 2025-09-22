using Application.DTO.Content.Preferences;
using Application.Interface.Mappers;
using Domain.Models.Contents;

namespace Application.Utils;

public class GenreMapper : IGenreMapper
{
    public Genre ToEntity(GenreAddDTO dto)
    {
        return new Genre
        {
            Id = dto.id,
            Name = dto.name,
        };
    }
    public GenreAddDTO ToDTO(Genre genre)
    {
        return new GenreAddDTO(genre.Id, genre.Name);
    }
    public IEnumerable<Genre> ToEntities(IEnumerable<GenreAddDTO> dtos)
    {
        return dtos?.Select(ToEntity) ?? Enumerable.Empty<Genre>();
    }
}
