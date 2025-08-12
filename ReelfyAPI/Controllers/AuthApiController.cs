using Domain.Interface.Services.User;
using Microsoft.AspNetCore.Mvc;
using ReelfyAPI.Models;
using ReelfyAPI.Models.DTO;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

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

        [HttpGet("getallusers", Name = "GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _authServices.GetAllUsers();
            var updatedUsers = new List<UserResponseDTO>();

            foreach (var user in users)
            {
                var userLinks = new List<LinkDTO>{

                    new LinkDTO(
                        Href: Url.Link("GetUserById", new { id = user.Id }),
                        Rel: "self",
                        Method: "GET",
                        Title: "Obter usuário por ID",
                        Type: "application/json"
                    ),

                    new LinkDTO(
                        Href: Url.Link("Register", null),
                        Rel: "register",
                        Method: "POST",
                        Title: "Registrar novo usuário",
                        Type: "application/json"
                    ),

                    new LinkDTO(
                        Href: Url.Link("Login", null),
                        Rel: "login",
                        Method: "POST",
                        Title: "Fazer login",
                        Type: "application/json"
                    )
                };

                var updatedUser = user with { Links = userLinks };
                updatedUsers.Add(updatedUser);
            }

            return Ok(updatedUsers);
        }

        [HttpGet("{id}", Name = "GetUserById")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authServices.GetUserById(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado!");
            }
            return Ok(user);
        }

        [HttpPost("register",Name = "Register")]
        public async Task<IActionResult> Register(UserRegisterDTO request)
        {
            if (await _authServices.VerifyUser(request.Email))
            {
                return Conflict(new Response<UserRegisterDTO>(request, "Usuário já cadastrado.", 0));
            }

            var userToCreate = new UserRegisterDTO(request.Email,request.Name, request.Password, request.Age, request.PhoneNumber);

            var createdUser = await _authServices.Register(userToCreate);
            var token = _authServices.CreateToken(createdUser);

            var response = new
            {
                data = createdUser,
                token = token,
                links = new List<LinkDTO>
                {
                    new LinkDTO(Url.Link("GetUserById", new { id = createdUser.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                    new LinkDTO(Url.Link("UpdateUser", new { id = createdUser.Id }), "update", "PUT", "Atualizar senha do usuário"),
                    new LinkDTO(Url.Link("DeleteUser", new { id = createdUser.Id }), "delete", "DELETE", "Deletar usuário"),
                    new LinkDTO(Url.Link("Login", new { email = createdUser.Email }), "login", "POST", "Fazer login")
                }
            };
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, response);

        }
        [HttpGet("/health")]
        public IActionResult HealthCheck() => Ok("API tá viva!");

        [HttpPost("login",Name ="Login")]
        public async Task<IActionResult> Login(UserLoginDTO request)
        {

            var user = await _authServices.Login(request);
            if (user == null)
            {
                return Unauthorized("Usuário ou senha inválidos!");
            }

            var token = _authServices.CreateToken(user);

            if (token  == null) {
                return Unauthorized(new Response<UserResponseDTO>(user,"Sem autorização. Seu token não foi salvo corretamente.",401));
            }

            var response = new
            {
                data = user,
                token = token,
                links = new List<LinkDTO>
                {
                    new LinkDTO(Url.Link("GetUserById", new { id = user.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                    new LinkDTO(Url.Link("UpdateUser", new { id = user.Id }), "update", "PUT", "Atualizar senha do usuário"),
                    new LinkDTO(Url.Link("DeleteUser", new { id = user.Id }), "delete", "DELETE", "Deletar usuário")
                }
            };
            return Ok(response);

        }

        [HttpDelete("DeleteUser/{id:int}", Name = "DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (id <= 0)
            {
                return BadRequest("Id inválido!");
            }

            var user = await _authServices.GetUserById(id);
            if (user == null)
            {
                return NotFound("Usuário não encontrado!");
            }

            var response = new
            {
                data = user,
                links = new List<LinkDTO>
                {
                    new LinkDTO(Url.Link("GetUserById", new { id = user.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                    new LinkDTO(Url.Link("UpdateUser", new { id = user.Id }), "update", "PUT", "Atualizar usuário"),
                    new LinkDTO(Url.Link("DeleteUser", new { id = user.Id }), "delete", "DELETE", "Deletar usuário")

                }
            };
            await _authServices.DeleteUser(id);
            return Ok(response);
        }
    }
}
