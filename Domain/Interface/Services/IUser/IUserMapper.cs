using Domain.Models.DTO;
using ReelfyAPI.Models;
using ReelfyAPI.Models.DTO;

namespace Domain.Interface.Services.IUser
{
    public interface IUserMapper
    {
        //Single - User
        public User ToUser(UserResponseDTO userResponseDTO);
        public User ToUser(UpdatePasswordDTO updatePasswordDTO);
        public User ToUser(UserRegisterDTO userRegisterDTO);
        public User ToUser(UserLoginDTO userLoginDTO);
        public FavoriteDTO ToFavorite (User fav);

        //Single - UserResponseDTO
        public UserResponseDTO ToUserResponseDTO(User user);
        public UserResponseDTO ToUserResponseDTO(UserRegisterDTO userRegisterDTO);
        public UserResponseDTO ToUserResponseDTO(UserLoginDTO userLoginDTO);
        public UserResponseDTO ToUserResponseDTO(UpdatePasswordDTO updatePasswordDTO);

        //Single - UserRegisterDTO
        public UserRegisterDTO ToUserRegisterDTO(User user);
        public UserRegisterDTO ToUserRegisterDTO(UserResponseDTO userResponseDTO);
        public UserRegisterDTO ToUserRegisterDTO(UserLoginDTO userLoginDTO);
        public UserRegisterDTO ToUserRegisterDTO(UpdatePasswordDTO updatePasswordDTO);

        //Single - UserLoginDTO
        public UserLoginDTO ToUserLoginDTO(User user);
        public UserLoginDTO ToUserLoginDTO(UserResponseDTO userResponseDTO);
        public UserLoginDTO ToUserLoginDTO(UserRegisterDTO userRegisterDTO);
        public UserLoginDTO ToUserLoginDTO(UpdatePasswordDTO updatePasswordDTO);

        //Single - UpdatePasswordDTO
        public UpdatePasswordDTO ToUpdatePasswordDTO(User user);
        public UpdatePasswordDTO ToUpdatePasswordDTO(UserResponseDTO userResponseDTO);
        public UpdatePasswordDTO ToUpdatePasswordDTO(UserLoginDTO userLoginDTO);
        public UpdatePasswordDTO ToUpdatePasswordDTO(UserRegisterDTO userRegisterDTO);

        //DI - User
        public IEnumerable<User> ToUserList(IEnumerable<UserResponseDTO> userResponseDTO);
        public IEnumerable<User> ToUserLoginList(IEnumerable<UserLoginDTO> userLoginDTOs);
        public IEnumerable<User> ToUserRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs);
        public IEnumerable<User> ToUpdatePasswordList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs);

        //DI - UserResponseDTO
        public IEnumerable<UserResponseDTO> ToUserResponseDTOList(IEnumerable<User> users);
        public IEnumerable<UserResponseDTO> ToUserResponseLoginList(IEnumerable<UserLoginDTO> userLoginDTOs);
        public IEnumerable<UserResponseDTO> ToUserResponseRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs);
        public IEnumerable<UserResponseDTO> ToUpdatePasswordResponseList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs);

        //DI - UserRegisterDTO
        public IEnumerable<UserRegisterDTO> ToUserRegisterDTOList(IEnumerable<User> users);
        public IEnumerable<UserRegisterDTO> ToUserRegisterLoginList(IEnumerable<UserLoginDTO> userLoginDTOs);
        public IEnumerable<UserRegisterDTO> ToUserRegisterUpdatePasswordList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs);

        //DI - UserLoginDTO
        public IEnumerable<UserLoginDTO> ToUserLoginDTOList(IEnumerable<User> users);
        public IEnumerable<UserLoginDTO> ToUserLoginRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs);
        public IEnumerable<UserLoginDTO> ToUserLoginUpdatePasswordList(IEnumerable<UpdatePasswordDTO> updatePasswordDTOs);

        //DI - UpdatePasswordDTO
        public IEnumerable<UpdatePasswordDTO> ToUpdatePasswordDTOList(IEnumerable<User> users);
        public IEnumerable<UpdatePasswordDTO> ToUpdatePasswordRegisterList(IEnumerable<UserRegisterDTO> userRegisterDTOs);
        public IEnumerable<UpdatePasswordDTO> ToUpdatePasswordLoginList(IEnumerable<UserLoginDTO> userLoginDTOs);







    }
}
