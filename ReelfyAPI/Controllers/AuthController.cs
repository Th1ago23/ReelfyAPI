using Application.DTO.Users;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using ReelfyAPI.Application.DTO;

namespace ReelfyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthServices _authServices;

    public AuthController(IAuthServices authServices)
    {
        _authServices = authServices;
    }
    [HttpPost("Register", Name = "Register")]
    public async Task<IActionResult> Register(UserRegisterDTO request)
    {
        try
        {
            var userToCreate = new UserRegisterDTO(request.Email, request.Name, request.Password, request.Birthday, request.PhoneNumber);

            var createdUser = await _authServices.Register(userToCreate);
            var token = createdUser.token ?? throw new NullReferenceException();

            var response = new
            {
                data = createdUser,
                links = new List<LinkDTO>
            {
                new LinkDTO(Url.Link("GetUserById", new { id = createdUser.userResponseDTO.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                new LinkDTO(Url.Link("UpdateUser", new { id = createdUser.userResponseDTO.Id }), "update", "PUT", "Atualizar senha do usuário"),
                new LinkDTO(Url.Link("DeleteUser", new { id = createdUser.userResponseDTO.Id}), "delete", "DELETE", "Deletar usuário"),
                new LinkDTO(Url.Link("Login", new { email = createdUser.userResponseDTO.Email }), "login", "POST", "Fazer login")
            }
            };
            return Created("", response);
        }
        catch (NullReferenceException e)
        {
            return BadRequest("É necessário preencher todas as informações para realizar o cadastro.");
        }catch (ArgumentException e)
        {
            return BadRequest("O e-mail enviado já possui cadastro.");
        }
    }

    [HttpGet("/health")]
    public IActionResult HealthCheck() => Ok("API tá viva!");

    [HttpPost("Login", Name = "Login")]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {
        try
        {
            var user = await _authServices.Login(request);
            var response = new
            {
                data = user,
                nameLogin = user.userResponseDTO.name,
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

}