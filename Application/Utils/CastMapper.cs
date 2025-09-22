using Application.DTO.Content.Preferences;
using Domain.Interface.Mappers;
using Domain.Models.Contents;

namespace Application.Utils
{
    public class CastMapper : ICastMapper
    {
        public Cast ToEntity(CastAddDTO dto)
        {
            return new Cast
            {
                Name = dto.name,
                Id = dto.id,
            };
        }
        public CastAddDTO ToDTO(Cast cast)
        {
            return new CastAddDTO(cast.Id, cast.Name);
        }

        public IEnumerable<Cast> ToEntities(IEnumerable<CastAddDTO> dtos)
        {
            return dtos?.Select(ToEntity) ?? Enumerable.Empty<Cast>();
        }
    }
}
