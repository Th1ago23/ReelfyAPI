using Domain.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IFavoriteContentRepository
    {
        public Task Add(FavoriteContent content);
        public void Update(FavoriteContent content);
        public Task Delete(int id);
        public Task<FavoriteContent> GetById(int id);
        public Task<FavoriteContent?> GetByUserAndContentAsync(int userId, int contentId);
        public Task<IEnumerable<FavoriteContent>> GetAllAsync();
    }
}
