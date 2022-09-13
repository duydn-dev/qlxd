using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IStoreProcedureRepository<T>
    {
        Task<List<T>> GetListAsync(DynamicParameters dynamicParameters, string procedureName);
        Task<T> GetAsync(DynamicParameters dynamicParameters, string procedureName);
        Task<object> InsertOrUpdateOrDeleteAsync(DynamicParameters dynamicParameters, string procedureName);
    }
}
