using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
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
        private readonly IUrlHelper _urlHelper;
        
        public AuthApiController(IAuthServices authServices, IUrlHelper urlHelper)
        {
            _urlHelper = urlHelper;
            _authServices = authServices;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authServices.GetAllUsers();
            if (users != null && users.Any())
            {
                foreach (var user in users)
                {

                    user.Links.Add(
                        new LinkDTO
                        (_urlHelper.Link("GetUserById", new { id = user.Id }),
                            "self",
                            "GET",
                            "Detalhes do usuário",
                            "application/json"));
                    
                    user.Links.Add(
                        new LinkDTO
                        (
                            _urlHelper.Link("UpdateUser", new { id = user.Id }),
                            "update",
                            "PUT",
                            "Atualizar senha do usuário"
                        )
                    );

                    user.Links.Add(
                        new LinkDTO
                        (
                            _urlHelper.Link("DeleteUser", new { id = user.Id }),
                            "delete",
                            "DELETE",
                            "Deletar usuário"
                        )
                    );

                    user.Links.Add(
                        new LinkDTO
                        (
                            _urlHelper.Link("Login", new { email = user.Email }),
                            "login",
                            "POST",
                            "Fazer login"
                        )
                    );

                    user.Links.Add(
                        new LinkDTO
                        (
                            _urlHelper.Link("Register", new { email = user.Email }),
                            "register",
                            "POST",
                            "Registrar usuário"
                        )
                    );
                }

                return Ok(users);
            }

            return NotFound("Nenhum usuário encontrado!");
        }

        [HttpGet]
        [Route("GetUserById/{id:int}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authServices.GetUserById(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado!");
            }
            return Ok(user);
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register (UserRegisterDTO request)
        {
            if (await _authServices.UserExists(request.email)) {
                throw new Exception("Usuário já existe!");
            }
            
            var userToCreate = new UserRegisterDTO(request.email,request.password,request.age,request.phoneNumber)

            var createdUser = await _authServices.Register(userToCreate);
            return Ok(createdUser);
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {

                var user = await _authServices.Login(request);
                if (user == null)
                {
                    return Unauthorized("Usuário ou senha inválidos!");
                }

            var token = _authServices.CreateToken(user);

            return Ok(new
            {
                User = user,
                Token = token
            });

        }

        [HttpPut]
        [Route("UpdatePassword")]
        public async Task <IActionResult> UpdatePassword(UpdatePasswordDTO request)
        {
            var user = await _authServices.GetUserByEmail(request.Email);

            var updatedUser = await _authServices.UpdatePassword(user, request.NewPassword);
            return Ok(updatedUser);
        }

    }
}

