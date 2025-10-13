using Application.DTO.Users;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Mvc;

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

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login(UserLoginDTO request)
    {
        var serviceResponse = await _authServices.Login(request);

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }
    [HttpPost("UpdatePassword")]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordDTO userDetails)
    {
        var serviceResponse = await _authServices.UpdatePassword(userDetails);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpGet("/health")]
    public IActionResult HealthCheck() => Ok("API tá viva!");
}