using Application.DTO.Users;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Mvc;
using ReelfyAPI.Application.DTO;
using ReelfyAPI.Models;

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

    [HttpPost("Register")]
    public async Task<IActionResult> Register(UserRegisterDTO request)
    {
        var serviceResponse = await _authServices.Register(request);

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        var createdUser = serviceResponse.Data;
        var response = new
        {
            data = createdUser,
            links = new List<LinkDTO>
            {
                new LinkDTO(Url.Link("GetUserById", new { id = createdUser?.userResponseDTO?.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                new LinkDTO(Url.Link("UpdateUser", new { id = createdUser?.userResponseDTO?.Id }), "update", "PUT", "Atualizar senha do usuário"),
                new LinkDTO(Url.Link("DeleteUser", new { id = createdUser?.userResponseDTO?.Id}), "delete", "DELETE", "Deletar usuário"),
                new LinkDTO(Url.Link("Login", new { email = createdUser?.userResponseDTO?.Email }), "login", "POST", "Fazer login")
            }
        };

        return StatusCode(serviceResponse.StatusCode, response);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {
        var serviceResponse = await _authServices.Login(request);

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        var user = serviceResponse.Data;
        var response = new
        {
            data = user,
            nameLogin = user?.User?.name,
            links = new List<LinkDTO>
            {
                new LinkDTO(Url.Link("GetUserById", new { id = user?.User?.Id }), "self", "GET", "Detalhes do usuário", "application/json"),
                new LinkDTO(Url.Link("UpdateUser", new { id = user?.User?.Id }), "update", "PUT", "Atualizar senha do usuário"),
                new LinkDTO(Url.Link("DeleteUser", new { id = user?.User?.Id }), "delete", "DELETE", "Deletar usuário")
            }
        };

        return StatusCode(serviceResponse.StatusCode, response);
    }

    [HttpGet("/health")]
    public IActionResult HealthCheck() => Ok("API tá viva!");
}