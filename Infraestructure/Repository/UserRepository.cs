using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Domain.Models.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ReelfyAPI.Data;


namespace Infraestructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        //public async Task<User> FindFavorite()
        //{
        //    var ur = _acessor.Id ?? throw new ArgumentNullException();
        //    var cacheKey = $"UserFavorites_{ur}";

        //    if (_cache.TryGetValue(cacheKey, out User cachedUser)) return cachedUser;

        //    var user = await _context
        //     .Users
        //     .Include(u => u.Movies)
        //     .FirstOrDefaultAsync(u => u.Id == ur)
        //     ?? throw new ArgumentNullException();

        //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        //    _cache.Set(cacheKey, user, cacheEntryOptions);

        //    return user;
        //}

        public async Task<bool> RemoveFavorite(int ContentId, int userId)
        {
            var user = await GetById(userId);

            var movie = user.Contents.FirstOrDefault(u => u.Id == ContentId) ?? throw new NullReferenceException("Não foi encontrado nenhum conteúdo com este Id.");

            user.Contents.Remove(movie);

            return true;

        }

        public async Task<User> FindFavorite(int id)
        {
            var user = await _context
             .Users
             .Include(u => u.Contents)
             .FirstOrDefaultAsync(u => u.Id == id)
             ?? throw new ArgumentNullException();

            return user;
        }
        public async Task<User> Add(User user)
        {
            try
            {
                if (user == null)
                {
                    throw new ArgumentNullException();

                }
                else if (await UserExists(user.Email))
                {
                    throw new InvalidOperationException($"Email {user.Email} já cadastrado.");
                }

                await _context.Users.AddAsync(user);

                return user;

            }
            catch (DbUpdateException e)
            {
                throw new Exception($"Erro ao registrar usuário. {e.Message}");
            }
        }

        public async Task<User> GetById(int id)
        {
            var user = await _context
                         .Users
                         .FirstOrDefaultAsync(u => u.Id == id)
                         ?? throw new ArgumentNullException();

            return user;
        }

        public async Task<User> GetByEmail(string email)
        {
            var user = await _context
                                .Users
                                .FirstOrDefaultAsync(u => u.Email == email) ??
                                throw new ArgumentNullException();

            return user;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var users = await _context.Users
                .Include(u => u.Preference)
                    .ThenInclude(p => p.Casts)
                .Include(u => u.Preference)
                    .ThenInclude(p => p.Crews)
                .Include(u => u.Preference)
                    .ThenInclude(p => p.Genres)
                .Include(u => u.Preference)
                    .ThenInclude(p => p.Streamings)
                .Include(u => u.Contents)
                .ToListAsync();

            return users;
        }

        public void Update(User user)
        {
            _context.Users
                .Update(user);

            user.UpdatedAt = DateTime.UtcNow;
        }

        public void Delete(User user)
        {
            _context.Users
                .Remove(user);

        }

        public async Task<bool> UserExists(string email)
        {
            return await _context
                            .Users
                            .AnyAsync(u => u.Email == email);
        }
    }
}
