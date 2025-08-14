using Application.Utils;
using Domain.Interface.Services.IUser;
using Domain.Interface.Services.Movie;
using Domain.Models.DTO;
using Microsoft.Extensions.Configuration;
using ReelfyAPI.Models.DTO;
using System.Security.Claims;
namespace ReelfyAPI.Services
{
    public class AuthServices : IAuthServices
    {
        private readonly IUserRepository _context;
        private readonly IMovieRepository _movie;
        private readonly IConfiguration _configuration;
        private readonly IUserMapper _mapper;
        private readonly JwtFunctions _jwtFunctions;


        public AuthServices(IMovieRepository movie, IUserRepository context, IConfiguration configuration, IUserMapper mapper, JwtFunctions jwtFunctions)
        {
            _context = context;
            _movie = movie;
            _configuration = configuration;
            _mapper = mapper;
            _jwtFunctions = jwtFunctions;
        }

        public async Task<UserResponseDTO> Register(UserRegisterDTO user)
        {
            var userEntity = _mapper.ToUser(user);

            _jwtFunctions.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            await _context.Add(userEntity);


            return _mapper.ToUserResponseDTO(userEntity);
        }

        public async Task<UserResponseDTO> GetUserById(int id)
        {
            var us = await _context.GetById(id);

            return _mapper.ToUserResponseDTO(us);
        }

        public async Task<UserResponseDTO> GetUserByEmail(string email)
        {
            var userEntity = await _context.GetByEmail(email);

            return _mapper.ToUserResponseDTO(userEntity);
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsers()
        {
            var connection = _context
                                    .GetAll();
            return _mapper
                        .ToUserResponseDTOList(await connection);
        }

        public async Task<UserResponseDTO> Login(UserLoginDTO userDTO)
        {
            var user = await _context.GetByEmail(userDTO.Email);
            if (user == null || !VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
                return null;

            return _mapper.ToUserResponseDTO(user);
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

        public string CreateToken(UserResponseDTO user)
        {
            var claims = new List<Claim>
           {
               new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
               new Claim(ClaimTypes.Email, user.Email)
           };

            var tokenKey = _configuration.GetSection("AppSettings:Token").Value;
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
                .GetByEmail(updateDTO.Email);

            if (user == null)
            {
                return null;
            }
            else if (VerifyPasswordHash(updateDTO.CurrentPassword, user.PasswordHash, user.PasswordSalt) == false)
            {
                throw new UnauthorizedAccessException("Senha atual inválida.");
            }
            else if (string.IsNullOrEmpty(updateDTO.CurrentPassword) || string.IsNullOrEmpty(updateDTO.NewPassword))
            {
                return null;
            }


            _jwtFunctions.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            await _context.Update(user);

            return _mapper.ToUserResponseDTO(user);
        }

        public async Task<bool> DeleteUser(int id)
        {
            var user = await _context.GetById(id);

            if (user is null) throw new Exception("Usuário não encontrado.");

            await _context.Delete(user);
            return true;
        }

        public async Task<bool> VerifyUser(string email)
        {
            return await _context.UserExists(email);
        }

        public async Task<FavoriteDTO> GetFavorite (int id)
        {
            var user = await _context.FindFavorite(id);

            if (user == null)
            {
                return null;
            }

            return _mapper.ToFavorite(user);
        }

    
    }
}
