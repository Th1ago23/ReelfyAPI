using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infraestructure.Repository;

public class GenreRepository : IGenreRepository
{
    private readonly DataContext _context;

    public GenreRepository(DataContext context)
    {
        _context = context;
    }
    private IQueryable<Genre> Find()
    {
        return _context.Genres.AsQueryable();
    }

    public async Task Add(Genre genre)
    {
        await _context.Genres.AddAsync(genre);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        var genre = await Find().FirstOrDefaultAsync(i => i.Id == id) ?? throw new NullReferenceException();
        _context.Genres.Remove(genre);
        await _context.SaveChangesAsync();
    }
    public async Task<Genre> FindById(int id)
    {
        return await Find().FirstOrDefaultAsync(i => i.Id == id);
    }
}
