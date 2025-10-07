using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infrastructure.Repository;

public class FavoriteContentRepository:IFavoriteContentRepository
{
    private readonly DataContext _context;

    public FavoriteContentRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Add(FavoriteContent content)
    {
        await _context.AddAsync(content);
    }

    public async Task Delete(int id)
    {
        var content = await _context.FavoriteContents.FirstOrDefaultAsync(i=> i.Id == id);

        _context.FavoriteContents.Remove(content);
    }

    public async Task<FavoriteContent> GetById(int id)
    {
        var content = await _context.FavoriteContents.FirstOrDefaultAsync(i => i.Id == id);
        return content;
    }

    public void Update(FavoriteContent content)
    {
        _context.FavoriteContents.Update(content);
    }
    public async Task<FavoriteContent?> GetByUserAndContentAsync(int userId, int contentId)
    {
        return await _context.FavoriteContents
            .FirstOrDefaultAsync(fc => fc.UserId == userId && fc.ContentId == contentId);
    }

    public async Task<IEnumerable<FavoriteContent>> GetAllAsync()
    {
        return await _context.FavoriteContents
            .Include(fc => fc.Content) // opcional, se quiser dados do conteúdo
            .ToListAsync();
    }
}
