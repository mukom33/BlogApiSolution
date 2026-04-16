using System.Linq.Expressions;
using BlogApi.DataAccess.Abstract;
using BlogApi.DataAccess.Context;
using BlogApi.Domain.Entities;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Npgsql.Internal.TypeHandlers.FullTextSearchHandlers;

namespace BlogApi.DataAccess.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected readonly BlogApiContext _context;
        protected readonly DbSet<T> _dbset;
        public GenericRepository(BlogApiContext context)
        {
            _context = context;
            _dbset = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _dbset.AddAsync(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _dbset.ToListAsync();
        }

        public async Task<List<T>> GetListAsync(Expression<Func<T, bool>>? predicate,params Expression<Func<T, object>>[] includes)
        {
            var query = _dbset.AsQueryable();
            
            if(predicate != null)
                query = query.Where(predicate);
            if(includes != null)
             foreach (var include in includes)
            {
                query = query.Include(include);
            }

            return await query
                            .ToListAsync();
        }


        public async Task<T?> GetAsync(System.Linq.Expressions.Expression<Func<T, bool>> predicate)
        {
            return await _dbset.FirstOrDefaultAsync(predicate);
        }
        
        public void Remove(T entity)
        {
            _dbset.Remove(entity);
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate)
        {
            return  await _dbset.CountAsync(predicate);
        }

       

        public async Task<PagedResult<T>> GetPagedAsync(int page, int pageSize, Expression<Func<T, bool>>? filter = null, params Expression<Func<T, object>>[] includes)
        {
{
            if (page < 1)
                page = 1;

            if (pageSize < 1)
                pageSize = 10;

            IQueryable<T> query = _dbset;

            if (filter != null)
                query = query.Where(filter);

            if(includes != null)
             foreach (var include in includes)
            {
                query = query.Include(include);
            }

            var totalCount = await query.CountAsync();
            

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PagedResult<T>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
            };
}
        }       
    }
}