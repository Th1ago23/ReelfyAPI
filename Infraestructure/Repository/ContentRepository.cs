using Domain.Interface.Repository;
using Domain.Models.Contents;
using Domain.Models.Users;
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

        public void Attach(Content content)
        {
            _dataContext.Attach(content);
        }
        public async Task<Content> Add(Content content)
        {
            try
            {
                _dataContext.Add(content);

                return content;
            }
            catch (DbException e)
            {
                throw new ApplicationException("Ocorreu um erro ao favoritar o filme/série.", e); ;
            }
        }
        public void Update(Content content)
        {
            try
            {
                _dataContext.Contents.Update(content);

            }
            catch (DbException e)
            {
                throw new ApplicationException($"{e.Message}", e);
            }
        }

        public async Task Delete(Content content)
        {
            _dataContext.Contents.Remove(content);
        }

        //public async Task<int> Count()
        //{

        //    //return await _dataContext.Contents.CountAsync();
        //}

        public async Task<Content> Find(int id)
        {
            var content = _dataContext
                                    .Contents
                                    .FirstOrDefault(x => x.Id == id);
           return content;
        }


        public async Task<IEnumerable<Content>> FindAll()
        {
            var contents = await _dataContext
                                        .Contents
                                        .Include(i => i.FavoritedByUsers)
                                        .ToListAsync();

            if (contents.Count == 0)
            {
                return Enumerable.Empty<Content>();
            }

            return contents;
        }

        public async Task<Content> FindByName(string title)
        {
            var content = await _dataContext.Contents.FirstOrDefaultAsync(m => m.Title == title);

            return content;
        }
        public async Task<IEnumerable<Content>> GetFavoritedAndSeenByUserAsync(int userId)
        {
            return await _dataContext.Contents
                .Where(c => c.FavoritedByUsers.Any(f => f.UserId == userId) &&
                            c.SeenInLists.Any(s => s.UserId == userId))
                .ToListAsync();
        }
    }
}
