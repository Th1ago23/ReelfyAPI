using Application.DTO.Content;
using Domain.Models.Contents;

namespace Application.Interface.Mappers;

public interface ICrewMapper
{
    public Crew ToEntity (CrewAddDTO dTO);
    public IEnumerable<Crew> ToEntities (IEnumerable<CrewAddDTO> dtos);
}
