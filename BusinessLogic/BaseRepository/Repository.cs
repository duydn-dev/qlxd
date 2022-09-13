using Microsoft.EntityFrameworkCore;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.BaseRepository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly DuLieuXangDauContext _appDbContext;
        private DbSet<T> _dbSet;

        public Repository(DuLieuXangDauContext NeacDbContext)
        {
            _appDbContext = NeacDbContext;
            _dbSet = _appDbContext.Set<T>();
        }

        public async Task<T> Add(T entity)
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }

        public async Task<List<T>> AddRangeAsync(List<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public async Task<IQueryable<T>> AddRangeAsync(IQueryable<T> entity)
        {
            await _dbSet.AddRangeAsync(entity);
            return entity;
        }

        public async Task<T> Delete(T entity)
        {
            _dbSet.Remove(entity);
            return entity;
        }
        public async Task<bool> DeleteByExpression(Expression<Func<T, bool>> expression)
        {
            try
            {
                var source = GetByExpression(expression);
                _dbSet.RemoveRange(source);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetByExpression(Expression<Func<T, bool>> expression)
        {
            return _dbSet.Where(expression);
        }

        public async Task<T> Update(T entity)
        {
            _dbSet.Update(entity);
            return entity;
        }
        public async Task<List<T>> UpdateRangeAsync(List<T> entity)
        {
            _dbSet.UpdateRange(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> UpdateRangeAsync(IEnumerable<T> entity)
        {
            _dbSet.UpdateRange(entity);
            return entity;
        }

        public async Task<IQueryable<T>> UpdateRangeAsync(IQueryable<T> entity)
        {
            _dbSet.UpdateRange(entity);
            return entity;
        }
        public async Task<IQueryable<T>> DeleteRangeAsync(IQueryable<T> entity)
        {
            _dbSet.RemoveRange(entity);
            return entity;
        }

        public async Task<IEnumerable<T>> DeleteRangeAsync(IEnumerable<T> entity)
        {
            _dbSet.RemoveRange(entity);
            return entity;
        }
    }
}
