using Application.Interface.ContentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;

    public ContentController(IContentService contentService)
    {
        _contentService = contentService;
    }

    [Authorize]
    [HttpPost("Favorite", Name = "favorite")]
    public async Task<IActionResult> Favorite([FromBody] FavoriteRequest request)
    {
        var response = await _contentService.FavoriteAsync(request.Id);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpGet("{id}", Name = "GetContentDetails")]
    public async Task<IActionResult> GetContentDetails(int id)
    {
        var response = await _contentService.GetContentDetailsAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpGet("GetFavoriteInContext")]
    public async Task<IActionResult> GetFavoriteInContext()
    {
        var response = await _contentService.GetFavoritesAsync();
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpPost("RemoveFavorite/{id}", Name = "RemoveFavorite")]
    public async Task<IActionResult> RemoveContent(int id)
    {
        var response = await _contentService.UnfavoriteAsync(id);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpPut("MarkAlreadySeen/{contentId}/{result}")]
    public async Task<IActionResult> MarkSeen(int contentId, bool result)
    {
        var response = await _contentService.SetSeenStatusAsync(contentId, result);
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpGet("seen")]
    public async Task<IActionResult> GetMySeenContent()
    {
        var response = await _contentService.GetSeenAsync();
        return StatusCode(response.StatusCode, response);
    }

    [Authorize]
    [HttpGet("favorited-and-seen")]
    public async Task<IActionResult> GetMyFavoritedAndSeenContent()
    {
        var response = await _contentService.GetFavoritedAndSeenAsync();
        return StatusCode(response.StatusCode, response);
    }
}

public record FavoriteRequest(int Id);