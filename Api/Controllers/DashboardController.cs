using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.DataManagerDtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardRepository _dashboardRepository;
        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        [AuthenticationOnly]
        [HttpGet]
        [Route("get-list-count")]
        public async Task<Response<GetDashBoardDtqlDto>> GetListCount()
        {
           return await _dashboardRepository.GetList();
        }

        [AuthenticationOnly]
        [HttpPost]
        [Route("get-consumption-statistics")]
        public async Task<Response<object>> GetListStatisticalGasAsync([FromBody] GetConsumptionStatisticsRequestDto request)
        {
            return await _dashboardRepository.GetListStatisticalGasAsync(request);
        }
    }
}
