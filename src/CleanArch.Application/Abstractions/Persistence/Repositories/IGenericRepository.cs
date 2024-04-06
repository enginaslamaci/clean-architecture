using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CleanArch.Application.Abstractions.Persistence.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(Expression<Func<T, bool>> filter, bool? noTracking = default);
        T Get(Expression<Func<T, bool>> filter);
        Task<bool> AnyAsync(Expression<Func<T, bool>> filter);
        Task<IReadOnlyList<T>> GetAllAsync(Expression<Func<T, bool>> filter = null);
        IQueryable<T> GetAllQueryable(Expression<Func<T, bool>> filter = null);
        Task<IEnumerable<T>> GetPagedResponseAsync(int pageNumber, int pageSize, string sortBy = "", string sortDir = "", Expression<Func<T, bool>> filter = null, params Expression<Func<T, object>>[] includes);
        Task<int> CountAsync(Expression<Func<T, bool>> filter = null);
        Task<T> AddAsync(T entity);
        Task<int> AddRangeAsync(IEnumerable<T> entity);
        void AddAsync(T[] entities);
        Task UpdateAsync(T entity);
        Task DeleteAsync(T entity);
        Task<int> DeleteRangeAsync(IEnumerable<T> entity);
    }
}
