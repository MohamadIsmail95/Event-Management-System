using Domain.Dtos;
using Domain.Interfaces;
using Infrastructure.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Infrastructure.Data.Repositories
{
    public  class RepositoryBase<TEntity,TKey> : IAsyncRepository<TEntity,TKey> where TEntity : class
    {
        private readonly DbSet<TEntity> _dbSet;
        private readonly AppDbContext _dbContext;

        public RepositoryBase(AppDbContext dbContext)
        {
            _dbSet = dbContext.Set<TEntity>();
            _dbContext = dbContext;
        }
        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);  
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public  Task<bool> DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.FirstOrDefaultAsync(expression);
        }

        public Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> expression)
        {
            return _dbSet.Where(expression).ToListAsync();
        }

        public   Task<TEntity> UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
            return Task.FromResult(entity);
        }

        public virtual async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await _dbSet.FindAsync(id); 
        }

       public IQueryable<TEntity> GetListAsQuerableAsync(string input)
       {
            if (!string.IsNullOrEmpty(input))
            {
                var expression = ExpressionUtils.BuildPredicate<TEntity>("Name", "StartsWith",input);
                var data = _dbSet.Where(expression);
                return data;
            }
            return _dbSet.AsQueryable();
           
       }

       public IQueryable<TEntity> GetFilterListAsQuerableAsync(IQueryable<TEntity> data,ApiRequestFilter input)
        {
            var expression = ExpressionUtils.BuildPredicate<TEntity>(input.filterLists);
            var dataFiltered= data.Where(expression);
            return dataFiltered;
        }


    }
}
