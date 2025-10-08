using Application.DTO.Content;
using ReelfyAPI.Models;

namespace Application.Interface.ContentInterface
{
    public interface IContentService
    {
        Task<Response<bool>> FavoriteAsync(int contentId);
        Task<Response<bool>> UnfavoriteAsync(int contentId);
        Task<Response<bool>> SetSeenStatusAsync(int contentId, bool hasSeen);
        Task<Response<ContentDetailsDTO>> GetContentDetailsAsync(int contentId);
        Task<Response<IEnumerable<ContentSummaryDTO>>> GetFavoritesAsync();
        Task<Response<IEnumerable<ContentSummaryDTO>>> GetSeenAsync();
        Task<Response<IEnumerable<ContentSummaryDTO>>> GetFavoritedAndSeenAsync();
    }
}

