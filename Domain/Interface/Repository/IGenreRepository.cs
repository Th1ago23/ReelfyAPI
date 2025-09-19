using Domain.Models.Contents;

namespace Domain.Interface.Repository
{
    public interface IGenreRepository
    {
        public Task Add(Genre genre);
        public Task Delete(int id);
    }
}
