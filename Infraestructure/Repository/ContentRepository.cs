using Domain.Interface.Repository;
using Domain.Models.Contents;
using Domain.Models.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;
using System.Data.Common;

namespace Infraestructure.Repository
{
    public class ContentRepository : IContentRepository
    {
        private readonly DataContext _dataContext;


        public ContentRepository(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task<Content> Add(Content movie, User user)
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

        public async Task Delete(Content movie)
        {
            _dataContext.Contents.Remove(movie);
            await _dataContext.SaveChangesAsync();
        }

        public async Task<int> Count()
        {
            return await _dataContext.Contents.CountAsync();
        }

        public async Task<Content> Find(int id)
        {
            var movie = _dataContext.Contents.FirstOrDefault(x => x.Id == id)
                ?? throw new Exception($"Não foi possível buscar um filme com o id {id}.");

            return movie;
        }


        public async Task<IEnumerable<Content>> FindAll()
        {
            var movies = await _dataContext.Contents.ToListAsync();

            if (movies.Count == 0)
            {
                return Enumerable.Empty<Content>();
            }

            return movies;
        }

        public async Task<Content> FindByName(string title)
        {
            var movie = await _dataContext.Contents.FirstOrDefaultAsync(m => m.Title == title);

            return movie;
        }
    }
}
