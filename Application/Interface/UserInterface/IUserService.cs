using Application.DTO.Content;
using Application.DTO.Users;
using ReelfyAPI.Models;

namespace Application.Interface.UserInterface;

public interface IUserService
{
    Task<Response<UserSummaryDTO>> GetUserById(int id);
    Task<Response<UserSummaryDTO>> GetUserByEmail(string email);
    Task<Response<IEnumerable<UserSummaryDTO>>> GetAllUsers();
    Task<Response<bool>> DeleteUser(int id);
    Task<Response<bool>> VerifyUser(string email);
    Task<Response<FavoriteDTO>> GetFavorite(int id);
    Task<Response<FavoriteDTO>> GetFavoriteInContext();
    Task<Response<UserResponseDTO>> UpdateUser(UpdateUserDTO update);
    Task<Response<UserSummaryDTO>> TurnPreemium(int id, bool result);
    Task<Response<ContentAlreadySeensDTO>> ContentsAlreadySeens();
}