using Domain.Interface.Services.IUser;
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
        private readonly IAuthServices _authServices;

        public MovieController(IAuthServices authServices , IMovieService movieService)
        {
            _authServices = authServices;
            _movieService = movieService;
        }

        [Authorize]
        [HttpPost("favorite", Name = ("favorite"))]
        public async Task<IActionResult> Favorite(FavoriteMovieDTO request)
        {
            if (request is null) return BadRequest("Erro ao favoritar");

            await _movieService.Favorite(request);
            return Ok("Favoritado com Sucesso");
        }

        [HttpGet("{id}", Name =("GetFavorite"))]
        public async Task<IActionResult> GetFavorite (int id)
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
    
    }
}
