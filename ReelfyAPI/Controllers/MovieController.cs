using Domain.Interface.Services.Movie;
using Domain.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [Authorize]
        [HttpPost("favorite", Name = ("favorite"))]
        public async Task<IActionResult> Favorite(FavoriteMovieDTO request)
        {
            if (request != null)
            {
                await _movieService.Favorite(request);
                return Ok("Favoritado com Sucesso");
            }
            else
            {
                return BadRequest("Erro ao favoritar");
            }
        }

        [HttpGet]
    }
}
