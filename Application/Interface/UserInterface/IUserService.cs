using Application.DTO.Content;
using Application.DTO.Users;

namespace Application.Interface.UserInterface;

public interface IUserService
{
    public Task<UserSummaryDTO> GetUserById(int id);

    public Task<UserSummaryDTO?> GetUserByEmail(string email);

    public Task<IEnumerable<UserSummaryDTO>> GetAllUsers();

    public Task<FavoriteDTO> GetFavorite(int id);
    public Task<FavoriteDTO> GetFavoriteInContext();
    public Task<bool> VerifyUser(string email);

    public Task<bool> DeleteUser(int id);
    public Task<UserResponseDTO> UpdateUser(UpdateUserDTO update);
    public Task<UserSummaryDTO> TurnPreemium(int id, bool result);
}
