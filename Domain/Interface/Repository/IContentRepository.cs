using Domain.Models.Contents;
using Domain.Models.Users;

namespace Domain.Interface.Repository
{
    public interface IContentRepository
    {
        Task<Content> Add(Content movie, User user);
        Task Delete(Content movie);
        Task<int> Count();
        Task<Content> Find(int id);
        Task<IEnumerable<Content>> FindAll();
        Task<Content> FindByName(string name);

    }
}
