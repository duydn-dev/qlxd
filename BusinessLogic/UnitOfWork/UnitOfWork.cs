using BusinessLogic.BaseRepository;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private DuLieuXangDauContext _dbFactory;

        public DuLieuXangDauContext DbContext { get { return _dbFactory; } }
        public IRepository<T> GetRepository<T>() where T :class => new Repository<T>(_dbFactory);
        public IQueryable<T> GetAsQueryable<T>() where T :class => new Repository<T>(_dbFactory).GetAll();
        public UnitOfWork(DuLieuXangDauContext dbFactory)
        {
            _dbFactory = dbFactory;
        }
        public DuLieuXangDauContext GetDbContext()
        {
            return DbContext;
        }
        public async Task<int> SaveAsync()
        {
            return await _dbFactory.SaveChangesAsync();
        }
    }
}
