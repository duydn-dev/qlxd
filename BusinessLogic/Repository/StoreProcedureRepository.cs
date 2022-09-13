using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using BusinessLogic.Contracts;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class StoreProcedureRepository<T> : IStoreProcedureRepository<T> where T : class
    {
        private readonly string _conn;
        private readonly ILogRepository _logRepository;
        
        public StoreProcedureRepository(IConfiguration configuration, ILogRepository logRepository)
        {
            _conn = configuration.GetConnectionString("NeacDbContext");
            _logRepository = logRepository;
        }

        public async Task<T> GetAsync(DynamicParameters dynamicParameters, string procedureName)
        {
            try
            {
                using (var connection = new SqlConnection(_conn))
                {
                    if (connection.State == System.Data.ConnectionState.Open) await connection.CloseAsync();
                    await connection.OpenAsync();

                    var data = await connection.QueryFirstOrDefaultAsync<T>(procedureName,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        public async Task<List<T>> GetListAsync(DynamicParameters dynamicParameters, string procedureName)
        {
            try
            {
                using (var connection = new SqlConnection(_conn))
                {
                    if (connection.State == System.Data.ConnectionState.Open) await connection.CloseAsync();
                    await connection.OpenAsync();

                    var data = await connection.QueryAsync<T>(procedureName,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);
                    return data.ToList();
                }
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return new List<T>();
            }
        }

        public async Task<object> InsertOrUpdateOrDeleteAsync(DynamicParameters dynamicParameters, string procedureName)
        {
            try
            {
                using (var connection = new SqlConnection(_conn))
                {
                    if (connection.State == System.Data.ConnectionState.Open) await connection.CloseAsync();
                    await connection.OpenAsync();

                    var data = await connection.ExecuteScalarAsync(procedureName,
                        dynamicParameters,
                        commandType: CommandType.StoredProcedure);
                    return data;
                }
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return null;
            }
        }
    }
}
