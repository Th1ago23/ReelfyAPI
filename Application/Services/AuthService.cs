using Application.DTO.Content.Preferences;
using Application.DTO.Returns;
using Application.DTO.Users;
using Application.Interface.UserInterface;
using Application.Interface.UtilsInterface;
using Application.Services;
using Domain.Interface.Repository;
using ReelfyAPI.Models;
namespace ReelfyAPI.Services
{
    public class AuthService : IAuthServices
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _context;
        private readonly IUserMapper _mapper;
        private readonly IJwtService _jwtService;

        public AuthService(IUnitOfWork unitOfWork, IUserRepository context, IUserMapper mapper, IJwtService jwtService)
        {
            _unitOfWork = unitOfWork;
            _context = context;
            _mapper = mapper;
            _jwtService = jwtService;
        }

        public async Task<Response<UserResponseAuthDTO>> Register(UserRegisterDTO user)
        {
            if (user == null)
            {
                return new Response<UserResponseAuthDTO>(null, "É necessário inserir todos os dados.", 400);
            }

            var userEntity = _mapper.ToUser(user);

            if (!userEntity.ValidateAge())
            {
                return new Response<UserResponseAuthDTO>(null, "Apenas usuários acima de 10 anos podem se cadastrar.", 400);
            }

            if (await _context.UserExists(userEntity.Email))
            {
                return new Response<UserResponseAuthDTO>(null, "Já existe um usuário cadastrado com este e-mail.", 409);
            }

            _jwtService.CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;

            await _context.Add(userEntity);
            await _unitOfWork.CommitAsync();

            var token = _jwtService.CreateToken(userEntity.Id, userEntity.Email);
            var result = new UserResponseAuthDTO(userEntity.Id,userEntity.Name,userEntity.Email, userEntity.IsPreemium,token,null);

            return new Response<UserResponseAuthDTO>(result, "Cadastro realizado com sucesso!", 201);
        }
        public async Task<Response<UserResponseAuthDTO>> Login(UserLoginDTO userDTO)
        {
            var user = await _context.GetByEmail(userDTO.Email);

            if (user == null || !_jwtService.VerifyPasswordHash(userDTO.Password, user.PasswordHash, user.PasswordSalt))
            {
                return new Response<UserResponseAuthDTO>(null, "E-mail ou senha inválidos. Verifique se já possui uma conta.", 401);
            }

            var token = _jwtService.CreateToken(user.Id,user.Email);

            var preferenceResponse = new PreferenceResponseDTO(
            user.Preference.UserId,
            user.Preference.Id,
            user.Preference.Casts.Select(c => new CastAddDTO(c.Id, c.Name, c.ProfilePath)),
            user.Preference.Crews.Select(c => new CrewAddDTO(c.Id, c.Name, c.ProfilePath)),
            user.Preference.Genres.Select(g => new GenreAddDTO(g.Id, g.Name)),
            user.Preference.Streamings.Select(s => new StreamingAddDTO(s.Id, s.Name))
            );

            var result = new UserResponseAuthDTO(user.Id,user.Name,user.Email,user.IsPreemium,token, preferenceResponse);

            return new Response<UserResponseAuthDTO>(result, "Login bem-sucedido!", 200);
        }


        public async Task<Response<UserResponseAuthDTO>> UpdatePassword(UpdatePasswordDTO updateDTO)
        {
            var user = await _context.GetByEmail(updateDTO.Email);

            if (user == null)
            {
                return new Response<UserResponseAuthDTO>(null, "Usuário não encontrado.", 404);
            }

            if (!_jwtService.VerifyPasswordHash(updateDTO.CurrentPassword, user.PasswordHash, user.PasswordSalt))
            {
                return new Response<UserResponseAuthDTO>(null, "Senha atual inválida.", 401);
            }

            if (string.IsNullOrEmpty(updateDTO.CurrentPassword) || string.IsNullOrEmpty(updateDTO.NewPassword))
            {
                return new Response<UserResponseAuthDTO>(null, "Não foi possível alterar a senha. As senhas não podem ser nulas ou vazias.", 400);
            }

            _jwtService.CreatePasswordHash(updateDTO.NewPassword, out byte[] passwordHash, out byte[] passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            _context.Update(user);
            await _unitOfWork.CommitAsync();

            var token = _jwtService.CreateToken(user.Id, user.Email);

            var preferenceResponse = new PreferenceResponseDTO(
                user.Preference.UserId,
                user.Preference.Id,
                user.Preference.Casts.Select(c => new CastAddDTO(c.Id, c.Name, c.ProfilePath)),
                user.Preference.Crews.Select(c => new CrewAddDTO(c.Id, c.Name, c.ProfilePath)),
                user.Preference.Genres.Select(g => new GenreAddDTO(g.Id, g.Name)),
                user.Preference.Streamings.Select(s => new StreamingAddDTO(s.Id, s.Name))
             );

            var result = new UserResponseAuthDTO(user.Id, user.Name, user.Email, user.IsPreemium, token, preferenceResponse);

            return new Response<UserResponseAuthDTO>(result, "Senha alterada com sucesso!", 200);
        }
    }
}
