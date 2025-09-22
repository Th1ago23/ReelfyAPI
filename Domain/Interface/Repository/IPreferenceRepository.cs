using Domain.Models.Contents;

namespace Domain.Interface.Repository
{
    public interface IPreferenceRepository
    {
        public Task AddGenre(int id, Genre genre);
        public Task Add(Preference preference);
        public Task<Preference> GetPreferences(int id);
        public Task<Preference> GetPreferenceById(int id);
        public Task AddCast(int id, Cast cast);
        public Task AddStreaming(int id, Streaming streaming);
        public Task AddCrew(int id, Crew crew);
    }
}
