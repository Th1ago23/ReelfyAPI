using Domain.Models.Contents;

namespace Domain.Interface.Repository;

public interface ICrewRepository
{
    public Task Add(Crew crew);
    public Task Delete(int id);
    public Task<Crew> Find(int id);
}
