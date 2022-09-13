using Common.Dtos;
using Common.Dtos.WarningSystemDtos;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Common.Dtos.WarningSystemDtos.WarningSystemViewsDto;

namespace BusinessLogic.Contracts
{
    public interface IWarningSystemRepository
    {
        Task<Response<object>> InputedWarningAsync();
        Task<Response<object>> IllogicalDataAsync();
        Task<Response<object>> DistributionSystemDataAsync();
        Task<Response<object>> DeviantPriceDataAsync();
        Task<Response<object>> TotalAllocationsDataAsync();
    }
}
