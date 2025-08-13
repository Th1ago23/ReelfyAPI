using Domain.Models;
using ReelfyAPI.Models;

namespace Domain.Interface.Services.Movie
{
    public interface IMovieRepository
    {
        Task<FavoriteMovie> Add(FavoriteMovie movie, User user);
        Task Delete(FavoriteMovie movie);
        Task<int> Count();
        Task<FavoriteMovie> Find(int id);
        Task<IEnumerable<FavoriteMovie>> FindAll();
        Task<FavoriteMovie> FindByName(string name);

    }
}
