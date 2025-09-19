using Application.DTO.Content;
using Domain.Models.Contents;

namespace Application.Interface.Mappers;

public interface IStreamingMapper
{
    public Streaming ToEntity(StreamingAddDTO dto);
    public IEnumerable<Streaming> ToEntities(IEnumerable<StreamingAddDTO> dtos);
}
