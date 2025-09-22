using Application.DTO.Content.Preferences;
using Application.Interface.Mappers;
using Domain.Models.Contents;

namespace Application.Utils;

public class CrewMapper:ICrewMapper
{
    public Crew ToEntity (CrewAddDTO dto)
    {
        return new Crew
        {
            Id = dto.id,
            Name = dto.name,
        };
    }
    public CrewAddDTO ToDTO (Crew crew)
    {
        return new CrewAddDTO(crew.Id, crew.Name);
    }
    public IEnumerable<Crew> ToEntities (IEnumerable<CrewAddDTO> dtos)
    {
        return dtos?.Select(ToEntity) ?? Enumerable.Empty<Crew>();
    }
}
