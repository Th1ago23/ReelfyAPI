using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infrastructure.Repository;

public class FavoriteContentRepository : IFavoriteContentRepository
{
    private readonly DataContext _context;

    public FavoriteContentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAsync(FavoriteContent favoriteContent)
    {
        await _context.FavoriteContents.AddAsync(favoriteContent);
    }

    public void Delete(FavoriteContent favoriteContent)
    {
        _context.FavoriteContents.Remove(favoriteContent);
    }


    public void Update(FavoriteContent favoriteContent)
    {
        _context.FavoriteContents.Update(favoriteContent);
    }

    public async Task<FavoriteContent?> GetByIdAsync(int id)
    {
        return await _context.FavoriteContents.FindAsync(id);
    }

    public async Task<FavoriteContent?> GetByUserAndContentAsync(int userId, int contentId)
    {
        return await _context.FavoriteContents
            .FirstOrDefaultAsync(fc => fc.UserId == userId && fc.ContentId == contentId);
    }

    public async Task<bool> AnyAsync(int userId, int contentId)
    {
        return await _context.FavoriteContents
            .AnyAsync(fc => fc.UserId == userId && fc.ContentId == contentId);
    }
    public async Task<IEnumerable<Content>> GetFavoritesByUserAsync(int userId)
    {
        return await _context.FavoriteContents
            .Where(fc => fc.UserId == userId)
            .Include(fc => fc.Content)
            .Select(fc => fc.Content)
            .ToListAsync();
    }
    public async Task<bool> IsFavorited(int userId, int contentId)
    {
        return await _context.FavoriteContents
            .AnyAsync(i => i.UserId == userId && i.ContentId == contentId);
    }
    public async Task<HashSet<int>> GetFavoritedContentIdsByUserAsync(int userId, IEnumerable<int> contentIds)
    {
        return await _context.FavoriteContents
            .Where(fc => fc.UserId == userId && contentIds.Contains(fc.ContentId))
            .Select(fc => fc.ContentId)
            .ToHashSetAsync();
    }
}