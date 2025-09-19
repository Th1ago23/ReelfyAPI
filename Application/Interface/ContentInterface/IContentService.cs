using Application.DTO.Content;

namespace Application.Interface.ContentInterface
{
    public interface IContentService
    {
        Task<FavoriteContentDTO> Favorite(FavoriteContentDTO movie);
        Task<bool> RemoveFavorite(int id);

    }
}
