using Common.Dtos;
using Common.Dtos.ActionLogs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IActionLogsRepository
    {
        Task<Response<CreateActionLogDto>> CreateAsync(CreateActionLogDto request);
        Task<Response<GetListResponseModel<List<ActionLogsViewsDto>>>> GetActionLogsAsync(ActionLogGetPageDto request);
    }
}