using Application.DTO.Users;
using Application.Interface.Mappers;
using Application.Interface.UserInterface;
using Domain.Interface.HttpContext;
using Domain.Interface.Repository;
using ReelfyAPI.Models;

namespace Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IContextUser _contextUser;
    private readonly IUserMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IContextUser contextUser, IUserMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _contextUser = contextUser;
        _mapper = mapper;
    }

    public async Task<Response<UserSummaryDTO>> GetUserById(int id)
    {
        var user = await _unitOfWork.User.GetById(id);
        if (user == null)
            return new Response<UserSummaryDTO>(null, $"Usuário não encontrado com o ID {id}.", 404);

        var responseData = _mapper.ToSummaryDTO(user);
        return new Response<UserSummaryDTO>(responseData, "Usuário encontrado com sucesso.", 200);
    }

    public async Task<Response<UserSummaryDTO>> GetUserByEmail(string email)
    {
        var user = await _unitOfWork.User.GetByEmail(email);
        if (user == null)
            return new Response<UserSummaryDTO>(null, $"Usuário não encontrado com o e-mail {email}.", 404);

        var responseData = _mapper.ToSummaryDTO(user);
        return new Response<UserSummaryDTO>(responseData, "Usuário encontrado com sucesso.", 200);
    }

    public async Task<Response<IEnumerable<UserSummaryDTO>>> GetAllUsers()
    {
        var users = await _unitOfWork.User.GetAll();
        var listDTO = users.Select(_mapper.ToSummaryDTO);

        return new Response<IEnumerable<UserSummaryDTO>>(listDTO, "Lista de usuários recuperada com sucesso.", 200);
    }

    public async Task<Response<bool>> DeleteUser(int id)
    {
        var user = await _unitOfWork.User.GetById(id);
        if (user == null)
            return new Response<bool>(false, "Usuário não encontrado.", 404);

        _unitOfWork.User.Delete(user);
        await _unitOfWork.CommitAsync();
        return new Response<bool>(true, "Usuário deletado com sucesso.", 200);
    }

    public async Task<Response<bool>> VerifyUser(string email)
    {
        var userExists = await _unitOfWork.User.UserExists(email);
        var message = userExists ? "Usuário já existe." : "Usuário não existe.";
        return new Response<bool>(userExists, message, 200);
    }

    public async Task<Response<UserResponseDTO>> UpdateUser(UpdateUserDTO update)
    {
        var user = await _unitOfWork.User.GetById(_contextUser.Id);
        if (user == null)
            return new Response<UserResponseDTO>(null, "Usuário não encontrado.", 404);

        _mapper.UpdateEntity(user, update);
        user.UpdatedAt = DateTime.UtcNow;

        _unitOfWork.User.Update(user);
        await _unitOfWork.CommitAsync();

        var responseData = _mapper.ToUserResponseDTO(user);
        return new Response<UserResponseDTO>(responseData, "Usuário atualizado com sucesso.", 200);
    }

    public async Task<Response<UserSummaryDTO>> TurnPremium(int id, bool result)
    {
        var user = await _unitOfWork.User.GetById(id);
        if (user == null)
            return new Response<UserSummaryDTO>(null, $"Usuário não encontrado com o ID {id}.", 404);

        user.IsPreemium = result;
        _unitOfWork.User.Update(user);
        await _unitOfWork.CommitAsync();

        var responseData = _mapper.ToSummaryDTO(user);
        return new Response<UserSummaryDTO>(responseData, "Status de premium atualizado com sucesso.", 200);
    }
}