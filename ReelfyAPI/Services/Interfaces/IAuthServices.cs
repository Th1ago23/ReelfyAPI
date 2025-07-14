using ReelfyAPI.Models;

namespace ReelfyAPI.Services.Interfaces
{
    public interface IAuthServices
    {
        public Task<User> Register(User user, string password);
        public Task<User> Login(string email, string password);
        public Task<bool> UserExists(string email);
        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt);
        public string CreateToken(User user);
        public Task<User> GetUserById(int id);
        public Task<User> GetUserByEmail(string email);
        public Task<IEnumerable<User>> GetAllUsers();
    }
}
