using Application.DTO.Returns;
using Application.DTO.Users;
using ReelfyAPI.Models;

namespace Application.Interface.UserInterface;

public interface IAuthServices
{
    public Task<Response<UserResponseAuthDTO>> Register(UserRegisterDTO userDto);

    public Task<Response<UserResponseAuthDTO?>> Login(UserLoginDTO loginDto);
    public Task<Response<UserResponseAuthDTO>> UpdatePassword(UpdatePasswordDTO update);
}
