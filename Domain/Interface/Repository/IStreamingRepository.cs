using Domain.Models.Contents;

namespace Domain.Interface.Repository;

public interface IStreamingRepository
{
    public Task Add(Streaming streaming);
    public Task Delete(int id);
}
