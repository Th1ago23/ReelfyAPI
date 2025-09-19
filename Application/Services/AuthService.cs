using Application.DTO.Content;
using Application.DTO.Returns;
using Application.DTO.Users;
using Application.Interface.UserInterface;
using Application.Utils;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using Microsoft.Extensions.Configuration;
namespace ReelfyAPI.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IUserRepository _context;
        private readonly IUserMapper _mapper;
        private readonly IContextUser _contextUser;
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtFunctions _jwtFunctions;


        public AuthService(IContentRepository movie, IUserRepository context, IConfiguration configuration, IUserMapper mapper, JwtFunctions jwtFunctions, IContextUser contextUser, IUnitOfWork unitOfWork)
        {
            _contextUser = contextUser;
            _context = context;
            _mapper = mapper;
            _jwtFunctions = jwtFunctions;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRequestDTO> Register(UserRegisterDTO user)
        {
            if (user == null) throw new NullReferenceException("Necessário inserir todos os dados");
            var userEntity = _mapper.ToUser(user);

            _jwtFunctions.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            await _context.Add(userEntity);


            var response = _mapper.ToUserResponseDTO(userEntity);
            var token = _jwtFunctions.CreateToken(response);

            return new ResponseRequestDTO(response, token);
        }

        public async Task<UserResponseDTO> GetUserById(int id)
        {
            var us = await _context.GetById(id);

            if (us == null) throw new NullReferenceException($"Usuário não encontrado com o ID {id}");
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

        public async Task<ResponseRequestDTO> Login(UserLoginDTO userDTO)
        {
            var user = await _context.GetByEmail(userDTO.Email);
            if (user == null) throw new NullReferenceException("E-mail ou senha inválidos. Verifique se já possui uma conta");
            if (!VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt)) throw new ArgumentException("E-mail ou senha inválidos. Verifique se já possui uma conta");

            var response = _mapper.ToUserResponseDTO(user);
            var token = _jwtFunctions.CreateToken(response);

            return new ResponseRequestDTO(response, token);
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
        public async Task<UserResponseDTO> UpdatePassword(UpdatePasswordDTO updateDTO, string newPassword)
        {
            var user = await _context
                .GetByEmail(updateDTO.Email)
                ?? throw new NullReferenceException("Usuário não encontrado.");

            if (VerifyPasswordHash(updateDTO.CurrentPassword, user.PasswordHash, user.PasswordSalt) == false)
            {
                throw new ArgumentException("Senha atual inválida.");
            }
            else if (string.IsNullOrEmpty(updateDTO.CurrentPassword) || string.IsNullOrEmpty(updateDTO.NewPassword))
            {
                throw new NullReferenceException("Não foi possível alterar a senha.");
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

        public async Task<FavoriteDTO> GetFavorite(int id)
        {
            var user = await _context.FindFavorite(id)
                ?? throw new NullReferenceException("Usuário não encontrado");

            return _mapper.ToFavorite(user);
        }

        public async Task<FavoriteDTO> GetFavoriteInContext()
        {
            var user = await _context.FindFavorite(_contextUser.Id) ?? throw new ArgumentNullException();

            return _mapper.ToFavorite(user);

        }

        public async Task RemoveFavorite(int id)
        {
            var user = await _context.GetById(_contextUser.Id);

            if (user is null) throw new UnauthorizedAccessException("Usuário sem permissão");

            var contentToRemove = user.Contents.FirstOrDefault(i => i.Id == id);

            if (contentToRemove != null) user.Contents.Remove(contentToRemove);

            await _unitOfWork.CommitAsync();


        }


    }
}
