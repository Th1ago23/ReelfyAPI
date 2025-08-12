using Domain.Interface.Services.User;
using Infraestructure.Utils;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;
using ReelfyAPI.Models;


namespace Infraestructure.Repository
{
    public class UserRepository:IUserRepository
    {
        private readonly DataContext _context;
        private readonly JwtFunctions _jwtFunctions;

        public UserRepository(DataContext context, JwtFunctions jwtFunctions)
        {
            _context = context;
            _jwtFunctions = jwtFunctions;
        }

        public async Task<User> Add(User user)
        {
            try
            {
                if (user == null)
                {

                    throw new ArgumentNullException();

                } else if(await UserExists(user.Email))
                {
                    throw new InvalidOperationException($"Email {user.Email} já cadastrado.");
                }

                await _context.Users.AddAsync(user);

                return user;

            }catch (DbUpdateException e)
            {
                throw new Exception($"Erro ao registrar usuário. {e.Message}");
            }
        }

        public async Task<User> GetById (int id)
        {
           var user = await _context
                        .Users
                        .FirstOrDefaultAsync(u => u.Id == id)
                        ?? throw new ArgumentNullException();

            return user;
        }

        public async Task<User> GetByEmail( string email)
        {
            var user = await _context
                                .Users
                                .FirstOrDefaultAsync (u => u.Email == email)??
                                throw new ArgumentNullException();

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context
                                    .Users
                                    .Where(u => u.Email != null)
                                    .ToListAsync();

            return users;
        }
         
        public async Task Update (User user)
        {
            _context.Users
                .Update(user);

            user.UpdatedAt = DateTime.UtcNow;
            
            await _context
                .SaveChangesAsync();
        }

        public async Task Delete(User user)
        {
            _context.Users
                .Remove(user);
            
            await _context
                .SaveChangesAsync();
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context
                            .Users
                            .AnyAsync(u => u.Email == email);
        }
    }
}
