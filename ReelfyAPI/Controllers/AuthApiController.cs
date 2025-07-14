using Microsoft.AspNetCore.Mvc;
using ReelfyAPI.Models;
using ReelfyAPI.Models.DTO;
using ReelfyAPI.Services.Interfaces;

namespace ReelfyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthApiController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthApiController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authServices.GetAllUsers();
            return Ok(users);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register (UserRegisterDTO request)
        {
            if (await _authServices.UserExists(request.Email)) {
                throw new Exception("Usuário já existe!");
            }
            
            var userToCreate = new User
            {
                Email = request.Email,
                Age = request.Age
            };

            var createdUser = await _authServices.Register(userToCreate, request.Password);
            return Ok(createdUser);
        }
    }
}

