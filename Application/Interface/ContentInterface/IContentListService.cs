using Application.DTO.Content;
using ReelfyAPI.Models;

namespace Application.Interface.ContentInterface;

public interface IContentListService
{
    public Task<Response<ContentsListResponseDTO>> ListCreate(ContentListCreateDTO dto);
    public Task<Response<ContentsListResponseDTO>> AddContentToList(int contentId, int listId);
    public Task<Response<ContentsListResponseDTO>> RemoveContentoFromList(int contentId, int listId);
    public Task<Response<ContentsListResponseDTO>> DeleteContentList(int id);
    public Task<Response<ContentsListResponseDTO>> GetAllContentsFromList(int listId);
    public Task<Response<ContentFromListDTO>> GetContentFromList(int listId, int contentId);
}
