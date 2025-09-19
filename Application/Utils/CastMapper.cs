using Application.DTO.Content;
using Domain.Interface.Mappers;
using Domain.Models.Contents;

namespace Application.Utils
{
    public class CastMapper: ICastMapper
    {
        public Cast ToEntity (CastAddDTO dto)
        {
            return new Cast
            {
                Name = dto.name,
                Id = dto.id,
            };
        }
        
        public IEnumerable<Cast> ToEntities(IEnumerable<CastAddDTO> dtos)
        {
            return dtos?.Select(ToEntity) ?? Enumerable.Empty<Cast>();
        }
    }
}
