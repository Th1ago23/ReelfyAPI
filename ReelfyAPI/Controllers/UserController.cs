using Application.DTO.Users;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userServices;

    public UserController(IUserService userServices)
    {
        _userServices = userServices;
    }

    [HttpGet("getallusers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var serviceResponse = await _userServices.GetAllUsers();
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var serviceResponse = await _userServices.GetUserById(id);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDTO request)
    {
        var serviceResponse = await _userServices.UpdateUser(request);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpDelete("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var serviceResponse = await _userServices.DeleteUser(id);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPut("MarkPremium/{userId}/{isPremium}")] 
    public async Task<IActionResult> MarkPremium(int userId, bool isPremium)
    {
        var serviceResponse = await _userServices.TurnPremium(userId, isPremium);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }
}