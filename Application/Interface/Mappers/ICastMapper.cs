using Application.DTO.Content.Preferences;
using Domain.Models.Contents;

namespace Domain.Interface.Mappers;

public interface ICastMapper
{
    public Cast ToEntity(CastAddDTO dto);
    public CastAddDTO ToDTO(Cast cast);
    public IEnumerable<Cast> ToEntities(IEnumerable<CastAddDTO> dtos);
}
