using Domain.Interface.Repository;
using Domain.Models.Contents;
using Infraestructure.Repository;
using ReelfyAPI.Data;

namespace Infraestructure.Data;

public class UnitOfWork
{
    private readonly DataContext _context;
    
    public ICastRepository Cast { get;}
    public ICrewRepository Crew { get; }
    public IGenreRepository Genre{ get; }
    public IPreferenceRepository Preference { get; }
    public IContentRepository Content{ get; }
    public IStreamingRepository Streaming{ get; }
    public IUserRepository User{ get; }

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

}
