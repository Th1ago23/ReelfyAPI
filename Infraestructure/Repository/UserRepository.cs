using Domain.Interface.Repository;
using Domain.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using ReelfyAPI.Data;
using System.Security.Claims;


namespace Infraestructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _acessor;
        private readonly IMemoryCache _cache;

        public UserRepository(DataContext context)
        {
            //_cache = cache;
            //_acessor = acessor;
            _context = context;
        }

        //public async Task<User?> GetUserInContext()
        //{
        //    if (_acessor.HttpContext == null || _acessor.HttpContext.User == null)
        //        throw new UnauthorizedAccessException("HttpContext ou User não encontrado.");

        //    var claim = _acessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        //    if (claim == null)
        //        throw new UnauthorizedAccessException("Token JWT inválido ou expirado.");

        //    if (!int.TryParse(claim.Value, out int userId))
        //        throw new FormatException("O Claim do usuário não é um número válido.");

        //    var user = await GetById(userId);
        //    if (user == null)
        //        throw new KeyNotFoundException("Usuário não encontrado no banco.");

        //    return user;
        //}

        //public async Task<User> FindFavoriteInContext()
        //{
        //    var ur = await GetUserInContext() ?? throw new ArgumentNullException();
        //    var cacheKey = $"UserFavorites_{ur.Id}";

        //    if (_cache.TryGetValue(cacheKey, out User cachedUser)) return cachedUser;

        //    var user = await _context
        //     .Users
        //     .Include(u => u.Movies)
        //     .FirstOrDefaultAsync(u => u.Id == ur.Id)
        //     ?? throw new ArgumentNullException();

        //    var cacheEntryOptions = new MemoryCacheEntryOptions()
        //    .SetAbsoluteExpiration(TimeSpan.FromMinutes(10));
        //    _cache.Set(cacheKey, user, cacheEntryOptions);

        //    return user;
        //}

        //public async Task<bool> RemoveFavorite(int id)
        //{
        //    var user = await FindFavoriteInContext();

        //    var movie = user.Movies.FirstOrDefault(u => u.Id == id) ?? throw new NullReferenceException("Não foi encontrado nenhum filme com este Id.");

        //    user.Movies.Remove(movie);

        //    await _context.SaveChangesAsync();

        //    _cache.Remove($"UserFavorites_{user.Id}");

        //    return true;

        //}

        public async Task<User> FindFavorite(int id)
        {
            var user = await _context
             .Users
             .Include(u => u.Movies)
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
                await _context.SaveChangesAsync();

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
            var users = await _context
                                    .Users
                                    .Where(u => u.Email != null)
                                    .ToListAsync();

            return users;
        }

        public async Task Update(User user)
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
