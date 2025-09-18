
using Application.DTO.Users;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Mvc;
using ReelfyAPI.Application.DTO;

namespace ReelfyAPI.Controllers;

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

    [HttpPost("register", Name = "Register")]
    public async Task<IActionResult> Register(UserRegisterDTO request)
    {
        try
        {
            var userToCreate = new UserRegisterDTO(request.Email, request.Name, request.Password, request.Age, request.PhoneNumber);

            var createdUser = await _authServices.Register(userToCreate);
            var token = createdUser.token ?? throw new NullReferenceException();

            var response = new
            {
                data = createdUser,
                tokenRegister = token,
                links = new List<LinkDTO>
            {
                new LinkDTO(Url.Link("GetUserById", new { id = createdUser.userResponseDTO.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                new LinkDTO(Url.Link("UpdateUser", new { id = createdUser.userResponseDTO.Id }), "update", "PUT", "Atualizar senha do usuário"),
                new LinkDTO(Url.Link("DeleteUser", new { id = createdUser.userResponseDTO.Id}), "delete", "DELETE", "Deletar usuário"),
                new LinkDTO(Url.Link("Login", new { email = createdUser.userResponseDTO.Email }), "login", "POST", "Fazer login")
            }
            };
            return CreatedAtAction(nameof(GetUserById), new { id = createdUser.userResponseDTO.Id }, response);
        }
        catch (NullReferenceException e)
        {
            return BadRequest(e.Message);
        }

    }
    [HttpGet("/health")]
    public IActionResult HealthCheck() => Ok("API tá viva!");

    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {
        try
        {
            var user = await _authServices.Login(request);
            var token = user.token;
            var response = new
            {
                data = user,
                nameLogin = user.userResponseDTO.name,
                tokenLogin = token,
                links = new List<LinkDTO>
            {
                new LinkDTO(Url.Link("GetUserById", new { id = user.userResponseDTO.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                new LinkDTO(Url.Link("UpdateUser", new { id = user.userResponseDTO.Id}), "update", "PUT", "Atualizar senha do usuário"),
                new LinkDTO(Url.Link("DeleteUser", new { id = user.userResponseDTO.Id}), "delete", "DELETE", "Deletar usuário")
            }
            };
            return Ok(response);
        }
        catch (ArgumentException e)
        {
            return BadRequest(e.Message);

        }
        catch (InvalidOperationException e)
        {
            return NotFound(e.Message);
        }



    }

    [HttpDelete("DeleteUser/{id:int}", Name = "DeleteUser")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _authServices.GetUserById(id);
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
