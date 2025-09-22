using Application.DTO.Content.Preferences;
using Domain.Models.Contents;

namespace Application.Interface.Mappers;

public interface IStreamingMapper
{
    public Streaming ToEntity(StreamingAddDTO dto);
    public StreamingAddDTO ToDTO(Streaming streaming);
    public IEnumerable<Streaming> ToEntities(IEnumerable<StreamingAddDTO> dtos);
}
