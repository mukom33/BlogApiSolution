using System.Linq.Expressions;
using BlogApi.Domain.Entities;

namespace BlogApi.DataAccess.Abstract
{
    public interface IGenericRepository<T>where T:class
    {
        
        Task<T?> GetAsync(Expression<Func<T,bool>> predicate);
        
        Task AddAsync(T entity);
        void Remove(T entity);
        Task<int> SaveAsync();
        Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null,params Expression<Func<T, object>>[] includes);
        Task<List<T>> GetListAsync(Expression<Func<T, bool>>? predicate,params Expression<Func<T, object>>[] includes);
        Task<int> CountAsync(Expression<Func<T, bool>> predicate);
        
    }
}