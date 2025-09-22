using Domain.Interface.Repository;
using Infraestructure.Repository;
using ReelfyAPI.Data;

namespace Infraestructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;

    public ICastRepository Cast { get; }
    public ICrewRepository Crew { get; }
    public IGenreRepository Genre { get; }
    public IPreferenceRepository Preference { get; }
    public IContentRepository Content { get; }
    public IStreamingRepository Streaming { get; }
    public IUserRepository User { get; }

    public UnitOfWork(DataContext context)
    {
        _context = context;
        Cast = new CastRepository(_context);
        Crew = new CrewRepository(_context);
        Genre = new GenreRepository(_context);
        Preference = new PreferenceRepository(_context);
        Content = new ContentRepository(_context);
        Streaming = new StreamingRepository(_context);
        User = new UserRepository(_context);

    }
    public async Task<int> CommitAsync()
    {
        return await _context.SaveChangesAsync();
    }
    public void Dispose()
    {
        _context.Dispose();
    }


}
