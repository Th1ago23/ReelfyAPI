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
    Task<Response<UserResponseDTO>> UpdateUser(UpdateUserDTO update);
    Task<Response<UserPremiumDTO>> TurnPremium(int id, bool result);
}