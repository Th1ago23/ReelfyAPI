using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Application.Interface.UserInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;

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
            if (request is null) return BadRequest("Erro ao favoritar");

            await _contentService.Favorite(request);
            return Created();
        }

        [HttpGet("{id}", Name = ("GetFavorite"))]
        public async Task<IActionResult> GetFavorite(int id)
        {
            var user = await _userService.GetFavorite(id);

            if (user is null) return BadRequest();

            return Ok(user);
        }

        [Authorize]
        [HttpGet("GetFavoriteInContext")]
        public async Task<IActionResult> GetFavoriteInContext()
        {
            var user = await _userService.GetFavoriteInContext();
            if (user is null) return BadRequest();

            return Ok(user);
        }

        [Authorize]
        [HttpPost("RemoveFavorite/{id}", Name = ("RemoveFavorite"))]
        public async Task<IActionResult> RemoveContent(int id)
        {
            var response = await _contentService.Unfavorite(id);

            if (response is null || !response.Success)
                return BadRequest(response?.Message ?? "Erro ao remover favorito");

            return Ok("Filme desfavoritado com sucesso.");
        }
        [Authorize]
        [HttpPut("MarkAlreadySeen/{contentId}")]
        public async Task<IActionResult> MarkSeen(int contentId, bool result)
        {
            try
            {
                var request = await _contentService.MarkAlreadySeen(contentId, result);
                return Ok(request);
            }
            catch (DbException e)
            {
                return BadRequest("Os servidores estão fora no momento. Tente novamente mais tarde");
            }
            catch (NullReferenceException e)
            {
                return BadRequest("Não foi possível encontrar os dados do usuário ou conteúdo.");
            }
            catch (UnauthorizedAccessException e)
            {
                return Unauthorized("Usuário sem permissão. Faça o login novamente.");
            }


        }
    }
}
