using Microsoft.EntityFrameworkCore;
using NewBlog.Core.Entities;
using NewBlog.Data.Context;
using NewBlog.Data.Repositories.Abstraction;
using System.Linq.Expressions;

namespace NewBlog.Data.Repositories.Concretes
{
    public class Repository<T> : IRepository<T> where T : class, IEntityBase, new()
    {
        private readonly AppDbContext _context;

        public Repository(AppDbContext dbContext)
        {
            this._context = dbContext;
        }

        private DbSet<T> _table { get => _context.Set<T>(); }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>> predicate = null, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _table;

            if (predicate != null)
                query = query.Where(predicate);

            if (includeProperties.Any())
                foreach (var property in includeProperties)
                    query = query.Include(property);

            return await query.ToListAsync();

        }

        public async Task AddAsync(T entity)
        {
            await _table.AddAsync(entity);
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includeProperties)
        {
            IQueryable<T> query = _table;
            query = query.Where(predicate);

            if (includeProperties.Any())
                foreach (var property in includeProperties)
                    query = query.Include(property);

            return await query.SingleAsync();
        }

        public async Task<T> GetByGuidAsync(Guid id)
        {
            return await _table.FindAsync(id);
        }

        public async Task<T> UpdateAsync(T entity)
        {
            await Task.Run(() => _table.Update(entity));
            return entity;
        }

        public async Task DeleteAsync(T entity)
        {
            await Task.Run(() => _table.Remove(entity));
        }

        public async Task<bool> AnyAsync(Expression<Func<T, bool>> predicate)
        {
            return await _table.AnyAsync(predicate);
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> predicate = null)
        {
            if (predicate is not null)
                return await _table.CountAsync(predicate);

            return await _table.CountAsync();
        }
    }
}
