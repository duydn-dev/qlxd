using BusinessLogic.BaseRepository;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<T> GetRepository<T>() where T:class;
        IQueryable<T> GetAsQueryable<T>() where T:class;
        DuLieuXangDauContext GetDbContext();
        Task<int> SaveAsync();
    }
}
