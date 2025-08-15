using Domain.Interface.Services.IUser;
using Domain.Interface.Services.Movie;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;
using ReelfyAPI.Models;
using System.Data.Common;

namespace Infraestructure.Repository
{
    public class MovieRepository : IMovieRepository
    {
        private readonly DataContext _dataContext;
        private readonly IHttpContextAccessor _acessor;
        private readonly IUserRepository _userRepository;

        public MovieRepository(DataContext dataContext, IHttpContextAccessor acessor, IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _acessor = acessor;
            _dataContext = dataContext;
        }

        public async Task<FavoriteMovie> Add(FavoriteMovie movie, User user)
        {
            try
            {
                if (movie == null || user == null) throw new NullReferenceException();

                if (movie.User == null) movie.User = new List<User>();

                if (movie.User.Contains(user)) throw new Exception("Usuário já favoritou este filme");

                movie.User.Add(user);

                _dataContext.Add(movie);

                await _dataContext.SaveChangesAsync();

                return movie;
            }
            catch (DbException e)
            {
                throw new ApplicationException("Ocorreu um erro ao favoritar o filme/série.", e); ;
            }
        }

        public async Task Delete(FavoriteMovie movie)
        {
            _dataContext.Contents.Remove(movie);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _dataContext.Contents.CountAsync();
        }

        public async Task<FavoriteMovie> Find(int id)
        {
            var movie = _dataContext.Contents.FirstOrDefault(x => x.Id == id)
                ?? throw new Exception($"Não foi possível buscar um filme com o id {id}.");

            return movie;
        }


        public async Task<IEnumerable<FavoriteMovie>> FindAll()
        {
            var movies = await _dataContext.Contents.ToListAsync();

            if (movies.Count == 0)
            {
                return Enumerable.Empty<FavoriteMovie>();
            }

            return movies;
        }

        public async Task<FavoriteMovie> FindByName(string title)
        {
            var movie = await _dataContext.Contents.FirstOrDefaultAsync(m => m.Title == title);

            return movie;
        }
    }
}
