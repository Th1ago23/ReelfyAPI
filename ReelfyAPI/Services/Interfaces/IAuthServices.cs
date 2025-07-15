using Microsoft.AspNetCore.Mvc;
using ReelfyAPI.Models;
using ReelfyAPI.Models.DTO;

namespace ReelfyAPI.Services.Interfaces
{
    public interface IAuthServices
    {
        public Task<UserResponseDTO?> Register(UserRegisterDTO userDto);

        public Task<UserResponseDTO?> Login(UserLoginDTO loginDto);

        public Task<bool> UserExists(string email);

        public Task<UserResponseDTO?> GetUserById(int id);

        public Task<UserResponseDTO?> GetUserByEmail(string email);

        public Task<IEnumerable<UserResponseDTO>> GetAllUsers();

        public Task<UserResponseDTO?> UpdatePassword(UpdatePasswordDTO update, string newPassword);

        public Task<bool> DeleteUser(int id);
        public string CreateToken(UserResponseDTO user);


    }
}
