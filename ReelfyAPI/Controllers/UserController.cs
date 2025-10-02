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

    [HttpDelete("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var serviceResponse = await _userServices.DeleteUser(id);

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var serviceResponse = await _userServices.GetUserById(id);

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [Authorize]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO request)
    {
        var serviceResponse = await _userServices.UpdateUser(request);

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPut("MarkPreemium/{userId}/{isPreemium}")]
    public async Task<IActionResult> MarkPreemium(int userId, bool isPreemium)
    {
        var serviceResponse = await _userServices.TurnPreemium(userId, isPreemium);

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [Authorize]
    [HttpGet("ContentsAlreadySeen")]
    public async Task<IActionResult> ContentsAlreadySeen()
    {
        var serviceResponse = await _userServices.ContentsAlreadySeens();

        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }
}