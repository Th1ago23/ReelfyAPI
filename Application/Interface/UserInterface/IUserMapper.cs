using Application.DTO.Content;
using Application.DTO.Users;
using Domain.Models.Users;

namespace Application.Interface.UserInterface;

public interface IUserMapper
{
    User ToUser(UserRegisterDTO dto);
    UserResponseDTO ToUserResponseDTO(User user);
    UserSummaryDTO ToSummaryDTO(User user);
    void UpdateEntity(User user, UpdateUserDTO dto);
}
