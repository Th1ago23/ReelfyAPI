using ReelfyAPI.Data;
using ReelfyAPI.Services.Interfaces;
using ReelfyAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using AutoMapper;
using ReelfyAPI.Models.DTO;
using Azure.Core;
namespace ReelfyAPI.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly DataContext _context;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        

        public AuthServices(DataContext context, IConfiguration configuration, IMapper mapper)
        {
            _context = context;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context
                .Users
                .AnyAsync(u => u.Email == email);
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<UserResponseDTO> Register(UserRegisterDTO user)
        {
            if (await UserExists(user.email)) {

                throw new InvalidOperationException("Email já cadastrado.");
            
            }

            var userEntity = _mapper.Map<User>(user);

            CreatePasswordHash(user.password, out byte[] passwordHash, out byte[] passwordSalt);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            _context
                .Users
                .Add(userEntity);

            await _context
                .SaveChangesAsync();

            return _mapper.Map<UserResponseDTO>(userEntity);
        }

        public async Task<UserResponseDTO> GetUserById(int id)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(i =>  i.Id == id);
            if (userEntity == null)
            {
                return null;
            }

            return _mapper.Map<UserResponseDTO>(userEntity);

        }

        public async Task<UserResponseDTO> GetUserByEmail(string email)
        {
            var userEntity = await _context.Users.FirstOrDefaultAsync(i => i.Email == email);
            
            if(userEntity == null)
            {
                return null;
            }
            
            return _mapper.Map<UserResponseDTO>(userEntity);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers()
        {
            return _mapper.Map<IEnumerable<UserResponseDTO>>(
                await _context.Users.ToListAsync());
        }

        private bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac
                    .ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(storedHash);
            }
        }

        public async Task<UserResponseDTO> Login(UserLoginDTO userDTO)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDTO.Email);
            if (user == null || !VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return _mapper.Map<UserResponseDTO>(user);
        }

        public string CreateToken(UserResponseDTO user)
        {
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Email, user.Email)
           };

            var tokenKey = _configuration["TokenKey"];
            if (string.IsNullOrEmpty(tokenKey))
            {
                throw new ArgumentNullException(nameof(tokenKey), "TokenKey não pode ser nulo ou vazio.");
            }

            var key = new Microsoft
                .IdentityModel
                .Tokens
                .SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(tokenKey));

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

        public async Task<UserResponseDTO> UpdatePassword(UpdatePasswordDTO updateDTO, string newPassword)
        {
            var user = await _context
                .Users
                .FirstOrDefaultAsync(u => u.Email == updateDTO.Email);

            if (user == null)
            {
                return null;
            } else if ( VerifyPasswordHash(updateDTO.CurrentPassword, user.PasswordHash, user.PasswordSalt) == false)
            {
                throw new UnauthorizedAccessException("Senha atual inválida.");
            } else if (string.IsNullOrEmpty(updateDTO.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
            {
                return BadRequest("Senha atual e nova senha são obrigatórias.");
            }


            CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _context
                .Users
                .Update(user);

            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return _mapper.Map<UserResponseDTO>(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new Exception("Usuário não encontrado.");

            }
        }
    }
}
