using Application.DTO.Content;
using Domain.Models.Contents;

namespace Domain.Interface.Mappers;

public interface ICastMapper
{
    public Cast ToEntity(CastAddDTO dto);
    public IEnumerable<Cast> ToEntities(IEnumerable<CastAddDTO> dtos);
}
