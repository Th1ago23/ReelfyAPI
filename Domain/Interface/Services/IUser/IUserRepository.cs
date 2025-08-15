using ReelfyAPI.Models;

namespace Domain.Interface.Services.IUser
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<bool> RemoveFavorite(int id);
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task Update(User user);
        Task Delete(User user);
        Task<User> FindFavoriteInContext();
        Task<User> FindFavorite(int id);
        Task<bool> UserExists(string email);
        Task<User> GetUserInContext();
    }
}
