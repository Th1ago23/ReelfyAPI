using Domain.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IPreferenceRepository
    {
        public Task AddGenre(int id, Genre genre);
        public Task Add(Preference preference);
        public Task<Preference> GetPreferenceById(int id);
        public Task AddCast(int id, Cast cast);
        public Task AddStreaming(int id, Streaming streaming);
        public Task AddCrew(int id, Crew crew);
    }
}
