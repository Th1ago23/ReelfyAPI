using Application.DTO.Content.Preferences;
using Domain.Models.Contents;

namespace Application.Interface.Mappers;

public interface ICrewMapper
{
    public Crew ToEntity(CrewAddDTO dTO);
    public CrewAddDTO ToDTO(Crew crew);
    public IEnumerable<Crew> ToEntities(IEnumerable<CrewAddDTO> dtos);
}
