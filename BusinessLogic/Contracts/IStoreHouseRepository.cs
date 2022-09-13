using Common.Dtos;
using Common.Dtos.StoreHouseDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IStoreHouseRepository
    {
        Task<Response<object>> GetListAsync(StoreHouseGetListRequestDto request);
        Task<Response<CreateStoreHouseDto>> CreateAsync(CreateStoreHouseDto request);
        Task<Response<CreateStoreHouseDto>> EditAsync(CreateStoreHouseDto request);
        Task<Response<CreateStoreHouseDto>> DeleteAsync(int id);
        Task<Response<CreateStoreHouseDto>> GetByIdAsync(int id);
        Task<Response<List<StoreHouseCategory>>> GetListDropDown();
    }
}
