using Application.DTO.Content;
using Application.DTO.Users;
using Application.Interface.UserInterface;
using Domain.Models.Users;

namespace ReelfyAPI.Utils
{
    public class UserMapper : IUserMapper
    {
        public User ToUser(UserResponseDTO userResponseDTO)
        {
            if (userResponseDTO == null)
            {
                return null;
            }

            return new User
            {
                Email = userResponseDTO.Email,
                Id = userResponseDTO.Id,
                CreatedAt = userResponseDTO.CreatedAt
            };
        }

        public FavoriteDTO ToFavorite(User fav)
        {
            if (fav == null)
            {
                return null;
            }

            return new FavoriteDTO
            (
                fav.Id,
                fav.Email,
                fav.FavoriteContents?.Select(m => new FavoriteContentDTO(m.Id, m.Title, m.category, m.ImageUrl, m.AlreadySeen)).ToList() ?? new List<FavoriteContentDTO>()
            );
        }

        public User ToUser(UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                return null;
            }

            return new User
            {
                Email = updatePasswordDTO.Email,
                PasswordHash = null,
                PasswordSalt = null
            };
        }

        public User ToUser(UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null)
            {
                return null;
            }

            return new User
            {
                Name = userRegisterDTO.Name,
                Birthday = userRegisterDTO.Birthday,
                Email = userRegisterDTO.Email,
                PhoneNumber = userRegisterDTO.PhoneNumber,
                PasswordHash = null,
                PasswordSalt = null,
                CreatedAt = DateTime.UtcNow
            };
        }

        public User ToUser(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return null;
            }

            return new User
            {
                Email = userLoginDTO.Email,
                PasswordHash = null,
                PasswordSalt = null
            };
        }

        public UserResponseDTO ToUserResponseDTO(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserResponseDTO(user.Id, user.Name, user.Email, user.CreatedAt);
        }

        public UserResponseDTO ToUserResponseDTO(UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null)
            {
                return null;
            }

            return new UserResponseDTO(0, userRegisterDTO.Name, userRegisterDTO.Email, DateTime.UtcNow);
        }

        public UserResponseDTO ToUserResponseDTO(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return null;
            }

            return new UserResponseDTO(0, null, userLoginDTO.Email, null);
        }
        public UserResponseDTO ToUserResponseDTO(UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                return null;
            }

            return new UserResponseDTO(0, null, updatePasswordDTO.Email, null);
        }

        public UserRegisterDTO ToUserRegisterDTO(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserRegisterDTO(user.Email, user.Name, null, user.Birthday, user.PhoneNumber);
        }

        public UserRegisterDTO ToUserRegisterDTO(UserResponseDTO userResponseDTO)
        {
            if (userResponseDTO == null)
            {
                return null;
            }

            return new UserRegisterDTO(userResponseDTO.Email, null, null, DateOnly.FromDateTime(DateTime.Now), null);
        }
        public UserRegisterDTO ToUserRegisterDTO(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return null;
            }

            return new UserRegisterDTO(userLoginDTO.Email, null, null, DateOnly.FromDateTime(DateTime.Now), null);
        }

        public UserRegisterDTO ToUserRegisterDTO(UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                return null;
            }

            return new UserRegisterDTO(updatePasswordDTO.Email, null, null, DateOnly.FromDateTime(DateTime.Now), null);
        }

        public UserLoginDTO ToUserLoginDTO(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UserLoginDTO(user.Email, null);
        }
        public UserLoginDTO ToUserLoginDTO(UserResponseDTO userResponseDTO)
        {
            if (userResponseDTO == null)
            {
                return null;
            }

            return new UserLoginDTO(userResponseDTO.Email, null);
        }
        public UserLoginDTO ToUserLoginDTO(UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null)
            {
                return null;
            }

            return new UserLoginDTO(userRegisterDTO.Email, userRegisterDTO.Password);
        }
        public UserLoginDTO ToUserLoginDTO(UpdatePasswordDTO updatePasswordDTO)
        {
            if (updatePasswordDTO == null)
            {
                return null;
            }

            return new UserLoginDTO(updatePasswordDTO.Email, null);
        }

        public UpdatePasswordDTO ToUpdatePasswordDTO(User user)
        {
            if (user == null)
            {
                return null;
            }

            return new UpdatePasswordDTO(user.Email, null, null);
        }
        public UpdatePasswordDTO ToUpdatePasswordDTO(UserResponseDTO userResponseDTO)
        {
            if (userResponseDTO == null)
            {
                return null;
            }

            return new UpdatePasswordDTO(userResponseDTO.Email, null, null);
        }
        public UpdatePasswordDTO ToUpdatePasswordDTO(UserRegisterDTO userRegisterDTO)
        {
            if (userRegisterDTO == null)
            {
                return null;
            }

            return new UpdatePasswordDTO(userRegisterDTO.Email, null, null);
        }
        public UpdatePasswordDTO ToUpdatePasswordDTO(UserLoginDTO userLoginDTO)
        {
            if (userLoginDTO == null)
            {
                return null;
            }

            return new UpdatePasswordDTO(userLoginDTO.Email, null, null);
        }
        public IEnumerable<User> ToUserList(IEnumerable<UserResponseDTO> userResponseDTOs)
        {
            return userResponseDTOs?.Select(ToUser) ?? Enumerable.Empty<User>();
        }
        public IEnumerable<User> ToUserLoginList(IEnumerable<UserLoginDTO> userLoginDTOs)
        {
            return userLoginDTOs?.Select(ToUser) ?? Enumerable.Empty<User>();
        }
        public IEnumerable<User> ToUserRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs)
        {
            return userRegisterDTOs?.Select(ToUser) ?? Enumerable.Empty<User>();
        }
        public IEnumerable<User> ToUpdatePasswordList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs)
        {
            return updatePasswordDTOs?.Select(ToUser) ?? Enumerable.Empty<User>();
        }
        public IEnumerable<UserResponseDTO> ToUserResponseDTOList(IEnumerable<User> users)
        {
            return users?.Select(ToUserResponseDTO) ?? Enumerable.Empty<UserResponseDTO>();
        }
        public IEnumerable<UserResponseDTO> ToUserResponseLoginList(IEnumerable<UserLoginDTO> userLoginDTOs)
        {
            return userLoginDTOs?.Select(ToUserResponseDTO) ?? Enumerable.Empty<UserResponseDTO>();
        }
        public IEnumerable<UserResponseDTO> ToUserResponseRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs)
        {
            return userRegisterDTOs?.Select(ToUserResponseDTO) ?? Enumerable.Empty<UserResponseDTO>();
        }
        public IEnumerable<UserResponseDTO> ToUpdatePasswordResponseList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs)
        {
            return updatePasswordDTOs?.Select(ToUserResponseDTO) ?? Enumerable.Empty<UserResponseDTO>();
        }
        public IEnumerable<UserRegisterDTO> ToUserRegisterDTOList(IEnumerable<User> users)
        {
            return users?.Select(ToUserRegisterDTO) ?? Enumerable.Empty<UserRegisterDTO>();
        }
        public IEnumerable<UserRegisterDTO> ToUserRegisterLoginList(IEnumerable<UserLoginDTO> userLoginDTOs)
        {
            return userLoginDTOs?.Select(ToUserRegisterDTO) ?? Enumerable.Empty<UserRegisterDTO>();
        }
        public IEnumerable<UserRegisterDTO> ToUserRegisterUpdatePasswordList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs)
        {
            return updatePasswordDTOs?.Select(ToUserRegisterDTO) ?? Enumerable.Empty<UserRegisterDTO>();
        }
        public IEnumerable<UserLoginDTO> ToUserLoginDTOList(IEnumerable<User> users)
        {
            return users?.Select(ToUserLoginDTO) ?? Enumerable.Empty<UserLoginDTO>();
        }
        public IEnumerable<UserLoginDTO> ToUserLoginRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs)
        {
            return userRegisterDTOs?.Select(ToUserLoginDTO) ?? Enumerable.Empty<UserLoginDTO>();
        }
        public IEnumerable<UserLoginDTO> ToUserLoginUpdatePasswordList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs)
        {
            return updatePasswordDTOs?.Select(ToUserLoginDTO) ?? Enumerable.Empty<UserLoginDTO>();
        }
        public IEnumerable<UpdatePasswordDTO> ToUpdatePasswordDTOList(IEnumerable<User> users)
        {
            return users?.Select(ToUpdatePasswordDTO) ?? Enumerable.Empty<UpdatePasswordDTO>();
        }
        public IEnumerable<UpdatePasswordDTO> ToUpdatePasswordRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs)
        {
            return userRegisterDTOs?.Select(ToUpdatePasswordDTO) ?? Enumerable.Empty<UpdatePasswordDTO>();
        }
        public IEnumerable<UpdatePasswordDTO> ToUpdatePasswordLoginList(IEnumerable<UserLoginDTO> userLoginDTOs)
        {
            return userLoginDTOs?.Select(ToUpdatePasswordDTO) ?? Enumerable.Empty<UpdatePasswordDTO>();
        }
    }
}