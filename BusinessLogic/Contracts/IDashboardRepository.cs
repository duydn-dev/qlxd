using Common.Dtos;
using Common.Dtos.DataManagerDtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IDashboardRepository
    {
        Task<Response<GetDashBoardDtqlDto>> GetList();
        Task<Response<object>> GetListStatisticalGasAsync(GetConsumptionStatisticsRequestDto request);
    }
}