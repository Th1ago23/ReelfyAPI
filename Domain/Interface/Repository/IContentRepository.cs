using Domain.Models.Contents;
using Domain.Models.Users;

namespace Domain.Interface.Repository
{
    public interface IContentRepository
    {
        Task<Content> Add(Content content, User user);
        Task Delete(Content content);
        void Update(Content content);
        //Task<int> Count();
        Task<Content> Find(int id);
        Task<IEnumerable<Content>> FindAll();
        Task<Content> FindByName(string name);

    }
}
