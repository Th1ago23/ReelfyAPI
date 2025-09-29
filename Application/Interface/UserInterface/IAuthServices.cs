using Application.DTO.Returns;
using Application.DTO.Users;
using ReelfyAPI.Models;

namespace Application.Interface.UserInterface;

public interface IAuthServices
{
    public Task<Response<ResponseRequestDTO>> Register(UserRegisterDTO userDto);

    public Task<Response<UserResponseLoginDTO?>> Login(UserLoginDTO loginDto);
    public Task<Response<UserResponseDTO>> UpdatePassword(UpdatePasswordDTO update, string newPassword);
}
