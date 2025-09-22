using Domain.Models.Users;

namespace Domain.Interface.Repository
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<bool> RemoveFavorite(int ContentId, int userId);
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        void Update(User user);
        void Delete(User user);
        Task<User> FindFavorite(int id);
        Task<bool> UserExists(string email);
    }
}
