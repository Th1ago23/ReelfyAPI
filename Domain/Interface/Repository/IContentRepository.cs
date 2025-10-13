using Domain.Models.Contents;
using Domain.Models.Users;

namespace Domain.Interface.Repository
{
    public interface IContentRepository
    {
        Task<Content> Add(Content content);
        Task Delete(Content content);
        void Update(Content content);
        void Attach(Content content);
        Task<Content> Find(int id);
        Task<IEnumerable<Content>> FindAll();
        Task<IEnumerable<Content>> GetFavoritedByUserAsync(int userId);

    }
}
