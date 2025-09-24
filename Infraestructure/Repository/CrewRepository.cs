using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infraestructure.Repository;

public class CrewRepository : ICrewRepository
{
    private readonly DataContext _context;

    public CrewRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Add(Crew crew)
    {
        await _context.SaveChangesAsync();
        await _context.Crews.AddAsync(crew);
    }
    public async Task Delete(int id)
    {
        var crew = await _context.Crews.FirstOrDefaultAsync(i => i.Id == id) ?? throw new NullReferenceException();
        _context.Crews.Remove(crew);
        await _context.SaveChangesAsync();
    }
    public async Task<Crew> Find (int id)
    {
        return await _context.Crews.FirstOrDefaultAsync(i => i.Id == id);
    }

}
