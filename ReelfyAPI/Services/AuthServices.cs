using ReelfyAPI.Data;
using ReelfyAPI.Services.Interfaces;
using ReelfyAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
namespace ReelfyAPI.Services
{
    public class AuthServices:IAuthServices
    {
        private readonly DataContext _context;
        private readonly IConfiguration configuration;

        public AuthServices(DataContext context, IConfiguration configuration)
        {
            _context = context;
            this.configuration = configuration;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context
                .Users
                .AnyAsync(u => u.Email != email);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> Register(User user, string password)
        {
            if (await UserExists(user.Email))
                return null;

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context
                .Users
                .Add(user);

            await _context
                .SaveChangesAsync();

            return user;
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context
                .Users
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context
                .Users
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllUsers()
        {
            return await _context
                .Users
                .ToListAsync();
        }

        public bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        public async Task<User> Login(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null || !VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public string CreateToken (User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };
            var key = new Microsoft
                .IdentityModel
                .Tokens
                .SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["TokenKey"]));
            
            var creds = new Microsoft
                .IdentityModel
                .Tokens
                .SigningCredentials(key, Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha512Signature);
            
            var tokenDescriptor = new Microsoft
                .IdentityModel
                .Tokens
                .SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(claims),
                        Expires = DateTime.Now.AddDays(7),
                        SigningCredentials = creds
                    };
            
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            
            return tokenHandler.WriteToken(token);

        }
    }
}
