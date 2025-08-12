using ReelfyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Services.User
{
    public interface IUserRepository
    {
        Task<User> Add(User user);
        Task<User> GetById(int id);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task Update(User user);
        Task Delete(User user);
        Task<bool> UserExists(string email);
    }
}
