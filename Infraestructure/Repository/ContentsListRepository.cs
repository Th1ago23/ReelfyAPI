using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;


namespace Infrastructure.Repository;

public class ContentsListRepository:IContentsListRepository
{
    private readonly DataContext _context;

    public ContentsListRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Add(ContentsList Contents)
    {
        await _context.AddAsync(Contents);
    }
    public async Task Delete(int id)
    {
        var contents = await _context.ContentsLists.FirstOrDefaultAsync(i => i.Id == id);
        _context.ContentsLists.Remove(contents);
    }
    public async Task<ContentsList> GetById(int id)
    {
        return await _context.ContentsLists.FirstOrDefaultAsync(i=>i.Id == id);
    }
}
