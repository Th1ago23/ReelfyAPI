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
        private readonly IContentService _movieService;
        private readonly IAuthServices _authServices;

        public ContentController(IAuthServices authServices, IContentService movieService)
        {
            _authServices = authServices;
            _movieService = movieService;
        }

        [Authorize]
        [HttpPost("favorite", Name = ("favorite"))]
        public async Task<IActionResult> Favorite(FavoriteContentDTO request)
        {
            if (request is null) return BadRequest("Erro ao favoritar");

            await _movieService.Favorite(request);
            return Created();
        }

        [HttpGet("{id}", Name = ("GetFavorite"))]
        public async Task<IActionResult> GetFavorite(int id)
        {
            var user = await _authServices.GetFavorite(id);

            if (user is null) return BadRequest();

            return Ok(user);
        }

        [Authorize]
        [HttpGet("getfavoriteincontext")]
        public async Task<IActionResult> GetFavoriteInContext()
        {
            var user = await _authServices.GetFavoriteInContext();
            if (user is null) return BadRequest();

            return Ok(user);
        }

        [Authorize]
        [HttpPost("RemoveFavorite/{id}", Name = ("RemoveFavorite"))]
        public async Task<IActionResult> RemoveMovie(int id)
        {
            var result = await _movieService.RemoveFavorite(id);

            if (result != true) return BadRequest("Erro ao remover favorito");

            return Ok("Filme desfavoritado com sucesso.");
        }
    }
}
