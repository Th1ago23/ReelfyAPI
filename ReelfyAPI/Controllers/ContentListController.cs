using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentListController : ControllerBase
    {
        private readonly IContentListService _service;

        public ContentListController(IContentListService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpPost("CreateList")]
        public async Task<IActionResult> CreateList(ContentListCreateDTO dto)
        {
            var serviceResponse = await _service.ListCreate(dto);

            if (!serviceResponse.Success) return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }

        [Authorize]
        [HttpDelete("DeleteList/{listId}")]
        public async Task<IActionResult> DeleteList(int listId)
        {
            var serviceResponse = await _service.DeleteContentList(listId);

            if (!serviceResponse.Success) return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }

        [Authorize]
        [HttpPut("AddContentToList/{listId}/{contentId}")]
        public async Task<IActionResult> AddContentToList(int listId, int contentId)
        {
            var serviceResponse = await _service.AddContentToList(contentId, listId);

            if (!serviceResponse.Success) return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }
        [Authorize]
        [HttpDelete("RemoveContentFromList/{listId}/{contentId}")]
        public async Task<IActionResult> RemoveContentFromList(int listId, int contentId)
        {
            var serviceResponse = await _service.RemoveContentoFromList(contentId, listId);

            if (!serviceResponse.Success) return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }
        [Authorize]
        [HttpGet("GetContentFromList/{listId}/{contentId}")]
        public async Task<IActionResult> GetContentFromList(int listId, int contentId)
        {
            var serviceResponse = await _service.GetContentFromList(listId, contentId);

            if (!serviceResponse.Success) return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }
        [Authorize]
        [HttpGet("GetAllContentsFromList/{listId}")]
        public async Task<IActionResult> GetAllContentsFromList(int listId)
        {
            var serviceResponse = await _service.GetAllContentsFromList(listId);

            if (!serviceResponse.Success) return StatusCode(serviceResponse.StatusCode, serviceResponse.Message);

            return StatusCode(serviceResponse.StatusCode, serviceResponse.Data);
        }
    }
}
