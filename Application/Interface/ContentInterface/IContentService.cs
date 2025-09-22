using Application.DTO.Content;

namespace Application.Interface.ContentInterface
{
    public interface IContentService
    {
        Task<FavoriteContentDTO> Favorite(FavoriteContentDTO movie);
        Task<FavoriteContentDTO> MarkAlreadySeen(int id, bool result);
        Task<bool> Unfavorite(int id);

    }
}
