using Domain.Interface.Services.Movie;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure.Repository
{
    public class MovieRepository:IMovieRepository
    {
        private readonly DataContext _dataContext;

        public MovieRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<FavoriteMovie> Add(FavoriteMovie movie)
        {
            try
            {
                if (movie == null)
                {
                    throw new NullReferenceException();
                }

                _dataContext.Movies.Add(movie);
                return movie;
            }
            catch (DbException e)
            {
                throw new Exception(e.Message);
            }            
        }

        public async Task Delete (FavoriteMovie movie)
        {
            _dataContext.Movies.Remove(movie);
        }

        public async Task<int> Count()
        {
            return await _dataContext.Movies.CountAsync();
        }

        public async Task<FavoriteMovie> Find (int id)
        {
            var movie = _dataContext.Movies.FirstOrDefault(x => x.Id == id)
                ?? throw new Exception($"Não foi possível buscar um filme com o id {id}.");

            return movie;
        }

        public async Task<IEnumerable<FavoriteMovie>> FindAll()
        {
            var movies = await _dataContext.Movies.ToListAsync();

            if (movies.Count == 0)
            {
                return Enumerable.Empty<FavoriteMovie>();
            }

            return movies;               
        }

        public async Task<FavoriteMovie> FindByName (string title)
        {
            var movie = await _dataContext.Movies.FirstOrDefaultAsync(m => m.Title == title);

            return movie;
        }
    }
}
