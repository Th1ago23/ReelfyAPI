using Application.DTO.Content;
using ReelfyAPI.Models;

namespace Application.Interface.ContentInterface
{
    public interface IContentService
    {
        Task<Response<ContentDTO>> FavoriteAsync(int contentId, string contentType);
        Task<Response<bool>> UnfavoriteAsync(int contentId);
        Task<Response<bool>> SetSeenStatusAsync(int contentId, bool hasSeen);
        Task<Response<ContentDetailsDTO>> GetContentDetailsAsync(int contentId);
        Task<Response<ContentsHomeDTO>> GetFavoritesInContext();
        Task<Response<IEnumerable<ContentSummaryDTO>>> GetSeenAsync();
    }
}

