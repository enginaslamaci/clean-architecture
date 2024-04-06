using CleanArch.Application.Abstractions.Persistence.Repositories;
using CleanArch.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CleanArch.Persistence.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _dbContext;

        public GenericRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> filter, bool? noTracking = default)
        {
            if (noTracking is true)
            {
                return await _dbContext.Set<T>().AsNoTracking().FirstOrDefaultAsync(filter);
            }

            return await _dbContext.Set<T>().FirstOrDefaultAsync(filter);
        }

        public virtual T Get(Expression<Func<T, bool>> filter)
        {
            return _dbContext.Set<T>().FirstOrDefault(filter);
        }

        public virtual async Task<bool> AnyAsync(Expression<Func<T, bool>> filter)
        {
            return await _dbContext.Set<T>().AnyAsync(filter);
        }

        public async Task<IEnumerable<T>> GetPagedResponseAsync(int pageNumber, int pageSize, string sortBy, string sortDir, Expression<Func<T, bool>> filter, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> query = _dbContext.Set<T>();

            if (includes != null)
            {
                query = includes.Aggregate(query,
                    (current, include) => current.Include(include));
            }

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (!string.IsNullOrEmpty(sortBy))
            {
                if (sortDir == "Desc" || sortDir == "desc")
                    query = query.OrderByDescending(r => EF.Property<T>(r, sortBy));
                else
                    query = query.OrderBy(r => EF.Property<T>(r, sortBy));
            }

            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<int> CountAsync(Expression<Func<T, bool>> filter)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.CountAsync();
        }

        public async Task<T> AddAsync(T entity)
        {
            await _dbContext.Set<T>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<int> AddRangeAsync(IEnumerable<T> entity)
        {
            _dbContext.Set<T>().AddRange(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public void AddAsync(T[] entities)
        {
            foreach (var item in entities)
            {
                _dbContext.Set<T>().Attach(item);
                _dbContext.Entry(item).State = EntityState.Added;
            }
            _dbContext.SaveChanges();
        }

        public async Task UpdateAsync(T entity)
        {
            var local = _dbContext.Set<T>()
                .Local
                .FirstOrDefault(entry => entry == entity);

            if (local is not null)
            {
                _dbContext.Entry(local).State = EntityState.Detached;
            }

            _dbContext.Entry(entity).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(T entity)
        {
            _dbContext.Set<T>().Remove(entity);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteRangeAsync(IEnumerable<T> entity)
        {
            _dbContext.Set<T>().RemoveRange(entity);
            return await _dbContext.SaveChangesAsync();
        }

        public async Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }
        public IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> filter = null)
        {
            IQueryable<T> query = _dbContext.Set<T>();
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return query.AsQueryable();
        }
    }
}