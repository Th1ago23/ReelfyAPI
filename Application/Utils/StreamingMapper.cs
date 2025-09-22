using Application.DTO.Content.Preferences;
using Application.Interface.Mappers;
using Domain.Models.Contents;

namespace Application.Utils;

public class StreamingMapper:IStreamingMapper
{
    public Streaming ToEntity (StreamingAddDTO dto)
    {
        return new Streaming
        {
            Id = dto.id,
            Name = dto.name
        };
    }
    public StreamingAddDTO ToDTO (Streaming streaming)
    {
        return new StreamingAddDTO(streaming.Id, streaming.Name);
    }
    public IEnumerable<Streaming> ToEntities (IEnumerable<StreamingAddDTO> dtos)
    {
        return dtos?.Select(ToEntity) ?? Enumerable.Empty<Streaming>();
    }
}
