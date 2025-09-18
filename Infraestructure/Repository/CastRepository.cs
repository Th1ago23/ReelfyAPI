using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infraestructure.Repository;

public class CastRepository:ICastRepository
{
    private readonly DataContext _context;

    public CastRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Add(Cast cast)
    {
        await _context.Casts.AddAsync(cast);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        var cast = await _context.Casts.FirstOrDefaultAsync(i => i.Id == id) ?? throw new NullReferenceException();
        _context.Casts.Remove(cast);
    }
}
