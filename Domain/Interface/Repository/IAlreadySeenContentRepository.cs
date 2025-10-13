using Domain.Models.Contents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interface.Repository
{
    public interface IAlreadySeenContentRepository
    {
        Task AddAsync(AlreadySeenContent alreadySeenContent);
        void Delete(AlreadySeenContent alreadySeenContent);
        Task<AlreadySeenContent?> GetByUserAndContentAsync(int userId, int contentId);
        Task<bool> AnyAsync(int userId, int contentId);
        Task<IEnumerable<Content>> GetSeenByUserAsync(int userId);
        Task<bool> IsAlreadySeen(int userId, int contentId);
        Task<HashSet<int>> GetSeenContentIdsByUserAsync(int userId, IEnumerable<int> contentIds);
    }
}
