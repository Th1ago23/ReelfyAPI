using Application.DTO.Content;
using Application.DTO.Returns;
using Application.DTO.Users;

namespace Application.Interface.UserInterface;

public interface IAuthServices
{
    public Task<ResponseRequestDTO?> Register(UserRegisterDTO userDto);

    public Task<ResponseRequestDTO?> Login(UserLoginDTO loginDto);

    public Task<UserResponseDTO?> GetUserById(int id);

    public Task<UserResponseDTO?> GetUserByEmail(string email);

    public Task<IEnumerable<UserResponseDTO>> GetAllUsers();

    public Task<UserResponseDTO?> UpdatePassword(UpdatePasswordDTO update, string newPassword);
    public Task<FavoriteDTO> GetFavorite(int id);
    public Task<FavoriteDTO> GetFavoriteInContext();
    public Task<bool> VerifyUser(string email);

    public Task<bool> DeleteUser(int id);


}
