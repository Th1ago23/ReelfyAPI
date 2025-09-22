using Application.DTO.Returns;
using Application.DTO.Users;
using Application.Interface.UserInterface;
using Application.Services;
using Domain.Interface.Repository;
namespace ReelfyAPI.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _context;
        private readonly IUserMapper _mapper;
        private readonly JwtService _jwtService;

        public AuthService(IUserRepository context, IUserMapper mapper, JwtService jwtFunctions, IUnitOfWork unitOfWork)
        {
            _context = context;
            _mapper = mapper;
            _jwtService = jwtFunctions;
            _unitOfWork = unitOfWork;
        }

        public async Task<ResponseRequestDTO> Register(UserRegisterDTO user)
        {
            if (user == null) throw new NullReferenceException("Necessário inserir todos os dados");
            var userEntity = _mapper.ToUser(user);
            if (!userEntity.ValidateAge()) throw new AccessViolationException("Apenas usuários acima de 10 anos podem se cadastrar");

            _jwtService.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            await _context.Add(userEntity);
            await _unitOfWork.CommitAsync();


            var response = _mapper.ToUserResponseDTO(userEntity);
            var token = _jwtService.CreateToken(response);

            return new ResponseRequestDTO(response, token);
        }
        public async Task<ResponseRequestDTO> Login(UserLoginDTO userDTO)
        {
            var user = await _context.GetByEmail(userDTO.Email);
            if (user == null) throw new NullReferenceException("E-mail ou senha inválidos. Verifique se já possui uma conta");
            if (!_jwtService.VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt)) throw new ArgumentException("E-mail ou senha inválidos. Verifique se já possui uma conta");

            var response = _mapper.ToUserResponseDTO(user);
            var token = _jwtService.CreateToken(response);

            return new ResponseRequestDTO(response, token);
        }

        public async Task<UserResponseDTO> UpdatePassword(UpdatePasswordDTO updateDTO, string newPassword)
        {
            var user = await _context
                .GetByEmail(updateDTO.Email)
                ?? throw new NullReferenceException("Usuário não encontrado.");

            if (_jwtService.VerifyPasswordHash(updateDTO.CurrentPassword, user.PasswordHash, user.PasswordSalt) == false)
            {
                throw new ArgumentException("Senha atual inválida.");
            }
            else if (string.IsNullOrEmpty(updateDTO.CurrentPassword) || string.IsNullOrEmpty(updateDTO.NewPassword))
            {
                throw new NullReferenceException("Não foi possível alterar a senha.");
            }
            _jwtService.CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Update(user);
            await _unitOfWork.CommitAsync();


            return _mapper.ToUserResponseDTO(user);
        }
    }
}
