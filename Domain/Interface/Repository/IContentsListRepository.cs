using Domain.Models.Contents;

namespace Domain.Interface.Repository;

public interface IContentsListRepository
{
    public Task Add(ContentsList Contents);
    public Task Delete(int id);
    public Task<ContentsList> GetById(int id);
}
