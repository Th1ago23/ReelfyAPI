using Application.DTO.Returns;
using Application.DTO.Users;

namespace Application.Interface.UserInterface;

public interface IAuthServices
{
    public Task<ResponseRequestDTO?> Register(UserRegisterDTO userDto);

    public Task<ResponseRequestDTO?> Login(UserLoginDTO loginDto);
    public Task<UserResponseDTO?> UpdatePassword(UpdatePasswordDTO update, string newPassword);
}
