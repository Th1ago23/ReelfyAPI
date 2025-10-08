using Application.DTO.Content;
using ReelfyAPI.Models;

namespace Application.Interface.ContentInterface;

public interface IContentListService
{
    Task<Response<ListDetailsDTO>> CreateListAsync(CreateContentListRequestDTO dto);
    Task<Response<bool>> DeleteListAsync(int listId);
    Task<Response<ListDetailsDTO>> AddContentToListAsync(int listId, int contentId);
    Task<Response<ListDetailsDTO>> RemoveContentFromListAsync(int listId, int contentId);
    Task<Response<IEnumerable<ListSummaryDTO>>> GetListsAsync();
    Task<Response<ListDetailsDTO>> GetListDetailsAsync(int listId);
}
