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

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
    }

    [HttpDelete("DeleteUser/{id:int}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var serviceResponse = await _userServices.DeleteUser(id);

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        return Ok(new { message = serviceResponse.Message });
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var serviceResponse = await _userServices.GetUserById(id);

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
    }

    [Authorize]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO request)
    {
        var serviceResponse = await _userServices.UpdateUser(request);

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
    }

    [HttpPost("MarkPreemium")]
    public async Task<IActionResult> MarkPreemium(int userId, bool IsPreemium)
    {
        var serviceResponse = await _userServices.TurnPreemium(userId, IsPreemium);

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
    }

    [Authorize]
    [HttpGet("ContentsAlreadySeen")]
    public async Task<IActionResult> ContentsAlreadySeen()
    {
        var serviceResponse = await _userServices.ContentsAlreadySeens();

        if (!serviceResponse.Success)
        {
            return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);
        }

        return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
    }
}