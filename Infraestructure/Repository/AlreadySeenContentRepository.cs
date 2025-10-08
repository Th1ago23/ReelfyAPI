using Domain.Interface.Repository;
using Domain.Models.Contents;
using Microsoft.EntityFrameworkCore;
using ReelfyAPI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class AlreadySeenContentRepository : IAlreadySeenContentRepository
    {
        private readonly DataContext _context;

        public AlreadySeenContentRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AlreadySeenContent alreadySeenContent)
        {
            await _context.AlreadySeenContents.AddAsync(alreadySeenContent);
        }

        public void Delete(AlreadySeenContent alreadySeenContent)
        {
            _context.AlreadySeenContents.Remove(alreadySeenContent);
        }

        public async Task<AlreadySeenContent?> GetByUserAndContentAsync(int userId, int contentId)
        {
            return await _context.AlreadySeenContents
                .FirstOrDefaultAsync(asc => asc.UserId == userId && asc.ContentId == contentId);
        }

        public async Task<bool> AnyAsync(int userId, int contentId)
        {
            return await _context.AlreadySeenContents
                .AnyAsync(asc => asc.UserId == userId && asc.ContentId == contentId);
        }
        public async Task<IEnumerable<Content>> GetSeenByUserAsync(int userId)
        {
            return await _context.AlreadySeenContents
                .Where(asc => asc.UserId == userId)
                .Include(asc => asc.Content)
                .Select(asc => asc.Content)
                .ToListAsync();
        }
    }
}
