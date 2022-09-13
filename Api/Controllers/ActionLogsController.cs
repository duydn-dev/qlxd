using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.ActionLogs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ActionLogsController : ControllerBase
    {
        private readonly IActionLogsRepository _actionLogsRepository;
        public ActionLogsController(IActionLogsRepository actionLogsRepository)
        {
            _actionLogsRepository = actionLogsRepository;
        }
        [AuthenticationOnly]
        [HttpPost]
        [Route("create")]
        public async Task<Response<CreateActionLogDto>> CreateAsync([FromBody]CreateActionLogDto request)
        {
            return await _actionLogsRepository.CreateAsync(request);
        }
        [AuthenticationOnly]
        [HttpPost]
        [Route("get-list")]
        public async Task<Response<GetListResponseModel<List<ActionLogsViewsDto>>>> GetListActionLogsAsync([FromBody]ActionLogGetPageDto request)
        {
            return await _actionLogsRepository.GetActionLogsAsync(request);
        }
    }
}
