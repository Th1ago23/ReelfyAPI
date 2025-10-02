using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
    {
        private readonly IContentService _contentService;
        private readonly IUserService _userService;

        public ContentController(IContentService contentService, IUserService userService)
        {
            _contentService = contentService;
            _userService = userService;
        }

        [Authorize]
        [HttpPost("Favorite", Name = ("favorite"))]
        public async Task<IActionResult> Favorite(FavoriteContentDTO request)
        {
            var response = await _contentService.Favorite(request);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("{id}", Name = ("GetFavorite"))]
        public async Task<IActionResult> GetFavorite(int id)
        {
            var response = await _userService.GetFavorite(id);
            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpGet("GetFavoriteInContext")]
        public async Task<IActionResult> GetFavoriteInContext()
        {
            var response = await _userService.GetFavoriteInContext();

            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPost("RemoveFavorite/{id}", Name = ("RemoveFavorite"))]
        public async Task<IActionResult> RemoveContent(int id)
        {
            var response = await _contentService.Unfavorite(id);

            return StatusCode(response.StatusCode, response);
        }

        [HttpGet("GetFavoritePerContentCount")]
        public async Task<IActionResult> GetCountPerContent()
        {
            var response = await _contentService.CountContents();

            return StatusCode(response.StatusCode, response);
        }

        [Authorize]
        [HttpPut("MarkAlreadySeen/{contentId}/{result}")]
        public async Task<IActionResult> MarkSeen(int contentId, bool result)
        {
            var request = await _contentService.MarkAlreadySeen(contentId, result);

            return StatusCode(request.StatusCode, request);

        }
    }
}
