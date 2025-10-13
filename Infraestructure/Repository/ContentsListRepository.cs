using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;
using System.Linq.Expressions;

namespace Infrastructure.Repository;

public class ContentsListRepository : IContentsListRepository
{
    private readonly DataContext _context;

    public ContentsListRepository(DataContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ContentsList contentList)
    {
        await _context.ContentsLists.AddAsync(contentList);
    }

    public void Delete(ContentsList contentList)
    {
        _context.ContentsLists.Remove(contentList);
    }

    public async Task<ContentsList?> GetByIdAsync(int id)
    {
        return await _context.ContentsLists
            .Include(l => l.Contents)
            .FirstOrDefaultAsync(l => l.Id == id);
    }
    public async Task<ContentsList?> GetByIdAndUserIdAsync(int listId, int userId)
    {
        return await _context.ContentsLists
            .Where(i=> i.UserId == userId)
            .Include(l => l.Contents)
            .FirstOrDefaultAsync(l => l.Id == listId);
    }

    public async Task<IEnumerable<ContentsList>> GetListsByUserAsync(int userId)
    {
        return await _context.ContentsLists
            .Include(l => l.Contents)
            .Where(l => l.UserId == userId)
            .ToListAsync();
    }

    public async Task<bool> AnyAsync(Expression<Func<ContentsList, bool>> predicate)
    {
        return await _context.ContentsLists.AnyAsync(predicate);
    }
}