using Application.DTO.Content;
using ReelfyAPI.Models;

namespace Application.Interface.ContentInterface
{
    public interface IContentService
    {
        Task<Response<FavoriteContentDTO>> Favorite(FavoriteContentDTO movie);
        Task<Response<FavoriteContentDTO>> MarkAlreadySeen(int id, bool result);
        Task<Response<bool>> Unfavorite(int id);
        Task<IEnumerable<FavoriteCountDTO>> CountContents();

    }
}
