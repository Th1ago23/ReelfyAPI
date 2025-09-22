using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;

namespace Infraestructure.Repository;

public class PreferenceRepository: IPreferenceRepository
{
    private DataContext _context;

    public PreferenceRepository(DataContext context)
    {
        _context = context;
    }

    public async Task Add(Preference preference)
    {
        await _context.Preferences.AddAsync(preference);
    }
    public async Task<Preference> GetPreferences(int id)
    {
        var preference = await _context

                                    .Preferences
                                    .Include(i=>i.Crews)
                                    .Include(i => i.Casts)
                                    .Include(i => i.Genres)
                                    .Include(i => i.Streamings)
                                    .FirstOrDefaultAsync(i => i.Id == id) ?? throw new NullReferenceException();
        return preference;
    }
    public async Task<Preference> GetPreferenceById(int id)
    {
        return await _context.Preferences.FirstOrDefaultAsync(i => i.Id == id) ?? throw new NullReferenceException();
    }
    public async Task AddGenre(int id, Genre genre)
    {
        var preference = await GetPreferenceById(id);
        preference.Genres.Add(genre);

    }
    public async Task AddCast(int id, Cast cast)
    {
        var preference = await GetPreferenceById(id) ?? throw new NullReferenceException();
        preference.Casts.Add(cast);
        
    }
    public async Task AddStreaming(int id, Streaming streaming)
    {
        var preference = await GetPreferenceById(id) ?? throw new NullReferenceException();
        preference.Streamings.Add(streaming);
        
    }
    public async Task AddCrew(int id, Crew crew)
    {
        var preference = await GetPreferenceById(id);
        preference.Crews.Add(crew);
        
    }

}
