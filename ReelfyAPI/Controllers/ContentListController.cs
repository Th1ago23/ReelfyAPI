using Application.DTO.Content;
using Application.Interface.ContentInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReelfyAPI.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class ContentListController : ControllerBase
{
    private readonly IContentListService _service;

    public ContentListController(IContentListService service)
    {
        _service = service;
    }

    [HttpPost("CreateList")]
    public async Task<IActionResult> CreateList([FromBody] CreateContentListRequestDTO dto)
    {
        var serviceResponse = await _service.CreateListAsync(dto);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpDelete("DeleteList/{listId}")]
    public async Task<IActionResult> DeleteList(int listId)
    {
        var serviceResponse = await _service.DeleteListAsync(listId);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpPut("AddContentToList/{listId}/{contentId}")]
    public async Task<IActionResult> AddContentToList(int listId, int contentId)
    {
        var serviceResponse = await _service.AddContentToListAsync(listId, contentId);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpDelete("RemoveContentFromList/{listId}/{contentId}")]
    public async Task<IActionResult> RemoveContentFromList(int listId, int contentId)
    {
        var serviceResponse = await _service.RemoveContentFromListAsync(listId, contentId);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpGet("GetContentFromList/{listId}/{contentId}")]
    public async Task<IActionResult> GetContentFromList(int listId, int contentId)
    {
        var listResponse = await _service.GetListDetailsAsync(listId);

        if (listResponse.StatusCode != 200 || listResponse.Data == null)
            return StatusCode(listResponse.StatusCode, listResponse);

        var content = listResponse.Data.Contents.FirstOrDefault(c => c.Id == contentId);

        if (content == null)
            return NotFound(new { Message = "Conteúdo não encontrado nesta lista." });

        return Ok(new { Message = "Conteúdo encontrado com sucesso", Data = content });
    }

    [HttpGet("GetAllContentsFromList/{listId}")]
    public async Task<IActionResult> GetAllContentsFromList(int listId)
    {
        var serviceResponse = await _service.GetListDetailsAsync(listId);
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }

    [HttpGet("GetLists")]
    public async Task<IActionResult> GetLists()
    {
        var serviceResponse = await _service.GetListsAsync();
        return StatusCode(serviceResponse.StatusCode, serviceResponse);
    }
}