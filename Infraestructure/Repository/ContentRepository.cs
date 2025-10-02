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

        public async Task<Content> Add(Content content, User user)
        {
            try
            {
                if (content == null || user == null) throw new NullReferenceException();

                if (content.FavoritedByUsers == null) content.FavoritedByUsers = new List<User>();

                if (content.FavoritedByUsers.Contains(user)) ;

                content.FavoritedByUsers.Add(user);

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

        //public async Task<IEnumerable<Content>> FindContentsAlreadSeens(int userId)
        //{
        //    var contents = await _dataContext.Contents.Where(i => i.AlreadySeen == true).FirstOrDefaultAsync(i=>i.InUserContentLists.FirstOrDefault(i=>i.Id == userId)).ToListAsync();
        //    var listContents = new List<Content>();

        //    foreach (var item in contents)
        //    {
        //        if (item.InUserContentLists.FirstOrDefault(i=>i.Id == userId))
        //    }
        //}

        public async Task<Content> FindByName(string title)
        {
            var content = await _dataContext.Contents.FirstOrDefaultAsync(m => m.Title == title);

            return content;
        }
    }
}
