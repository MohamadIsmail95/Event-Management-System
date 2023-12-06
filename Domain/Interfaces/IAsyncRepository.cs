using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IAsyncRepository<TEntity,TKey> where TEntity : class
    {
        Task<TEntity> AddAsync(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);
        Task<bool> DeleteAsync(TEntity entity);
        Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression);
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression);
        IQueryable<TEntity> GetListAsQuerableAsync(string input);
        IQueryable<TEntity> GetFilterListAsQuerableAsync(IQueryable<TEntity> data,ApiRequestFilter input);
        Task<TEntity> GetByIdAsync(TKey id);

    }
}
