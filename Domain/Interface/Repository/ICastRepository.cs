using Domain.Models.Contents;

namespace Domain.Interface.Repository
{
    public interface ICastRepository
    {
        public Task Add(Cast cast);
        public Task Delete(int id);
    }
}
