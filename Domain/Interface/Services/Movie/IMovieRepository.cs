using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Services.Movie
{
    public interface IMovieRepository
    {
        Task<FavoriteMovie> Add(FavoriteMovie movie);
        Task Delete(FavoriteMovie movie);
        Task<int> Count();
        Task<FavoriteMovie> Find(int id);
        Task<IEnumerable<FavoriteMovie>> FindAll();
        Task<FavoriteMovie> FindByName(string name);
        
    }
}
