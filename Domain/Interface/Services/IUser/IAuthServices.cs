using ReelfyAPI.Models.DTO;

namespace Domain.Interface.Services.IUser
{
    public interface IAuthServices
    {
        public Task<UserResponseDTO?> Register(UserRegisterDTO userDto);

        public Task<UserResponseDTO?> Login(UserLoginDTO loginDto);

        public Task<UserResponseDTO?> GetUserById(int id);

        public Task<UserResponseDTO?> GetUserByEmail(string email);

        public Task<IEnumerable<UserResponseDTO>> GetAllUsers();

        public Task<UserResponseDTO?> UpdatePassword(UpdatePasswordDTO update, string newPassword);
        public Task<>
        public Task<bool> VerifyUser(string email);

        public Task<bool> DeleteUser(int id);
        public string CreateToken(UserResponseDTO user);


    }
}
