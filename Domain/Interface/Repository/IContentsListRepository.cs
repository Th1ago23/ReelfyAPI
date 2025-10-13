using Domain.Models.Contents;
using System.Linq.Expressions;

namespace Domain.Interface.Repository;

public interface IContentsListRepository
{
    Task AddAsync(ContentsList contentList);
    void Delete(ContentsList contentList);
    Task<ContentsList?> GetByIdAsync(int id);
    Task<IEnumerable<ContentsList>> GetListsByUserAsync(int userId);
    Task<bool> AnyAsync(Expression<Func<ContentsList, bool>> predicate);
    Task<ContentsList?> GetByIdAndUserIdAsync(int listId, int userId);
}
