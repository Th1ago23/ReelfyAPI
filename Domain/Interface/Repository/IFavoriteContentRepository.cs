using Domain.Models.Contents;

namespace Domain.Interface.Repository;
public interface IFavoriteContentRepository
{
    Task AddAsync(FavoriteContent favoriteContent);
    void Delete(FavoriteContent favoriteContent);
    void Update(FavoriteContent favoriteContent);
    Task<FavoriteContent?> GetByIdAsync(int id);
    Task<FavoriteContent?> GetByUserAndContentAsync(int userId, int contentId);
    Task<bool> AnyAsync(int userId, int contentId);
    Task<IEnumerable<Content>> GetFavoritesByUserAsync(int userId);
    public Task<bool> IsFavorited(int userId, int contentId);
    public Task<HashSet<int>> GetFavoritedContentIdsByUserAsync(int userId, IEnumerable<int> contentIds);
}