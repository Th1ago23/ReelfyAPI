using Application.DTO.Users;
using Application.Interface.UserInterface;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReelfyAPI.Application.DTO;
using System;
using System.Data.Common;

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

    [HttpGet("getallusers", Name = "GetAllUsers")]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userServices.GetAllUsers();
        var updatedUsers = new List<UserSummaryDTO>();

        foreach (var user in users)
        {
            
            var userLinks = new List<LinkDTO>{
                new LinkDTO(
                    Href: Url.Link("GetUserById", new { id = user.id}),
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

    [HttpDelete("DeleteUser/{id:int}", Name = "DeleteUser")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var user = await _userServices.GetUserById(id);
        var response = new
        {
            data = user,
            links = new List<LinkDTO>
            {
                new LinkDTO(Url.Link("GetUserById", new { id = user.id }), "self", "GET", "Detalhes do usuário", "application/json"),
                new LinkDTO(Url.Link("UpdateUser", new { id = user.id }), "update", "PUT", "Atualizar usuário"),
                new LinkDTO(Url.Link("DeleteUser", new { id = user.id }), "delete", "DELETE", "Deletar usuário")
            }
        };
        await _userServices.DeleteUser(id);
        return Ok(response);
    }
    [HttpGet("{id}", Name = "GetUserById")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userServices.GetUserById(id);
        if (user == null)
        {
            return NotFound("Usuário não encontrado!");
        }
        return Ok(user);
    }

    [Authorize]
    [HttpPut("UpdateUser")]
    public async Task<IActionResult> UpdateUser(UpdateUserDTO request)
    {
        try
        {
            var response = await _userServices.UpdateUser(request);
            return Ok(response);
        } catch (UnauthorizedAccessException e)
        {
            return BadRequest($"Não foi possível atualizar os dados do usuário. Por favor, faça o login e tente novamente.\n {e.Message}");
        }    
    }
    [Authorize]
    [HttpPost("user/markPreemium")]
    public async Task<IActionResult> MarkPreemium(int userId, bool IsPreemium)
    {
        try
        {
            var response = await _userServices.TurnPreemium(userId, IsPreemium);
            return Ok(response);
        }catch(DbException e)
        {
            return BadRequest(e.Message);
        }
    }
}
