using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infraestructure.Repository;

public class StreamingRepository : IStreamingRepository
{
    private readonly DataContext _context;

    public StreamingRepository(DataContext context)
    {
        _context = context;
    }
    public async Task Add(Streaming streaming)
    {
        await _context.Streamings.AddAsync(streaming);
        await _context.SaveChangesAsync();
    }
    public async Task Delete(int id)
    {
        var stream = await _context.Streamings.FirstOrDefaultAsync(i => i.Id == id);
        await _context.SaveChangesAsync();
    }
}
