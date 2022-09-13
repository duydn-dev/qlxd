using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.WarningSystemDtos;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using static Common.Dtos.WarningSystemDtos.WarningSystemViewsDto;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý cảnh báo")]
    public class WarningSystemController : ControllerBase
    {
        public readonly IWarningSystemRepository _warningSystemRepository;
        public WarningSystemController(IWarningSystemRepository warningSystemRepository)
        {
            _warningSystemRepository = warningSystemRepository;
        }

        [RoleDescription("Xem danh sách đối tượng chưa nhập")]
        [HttpGet]
        [Route("get-list-not-inputed")]
        public async Task<Response<object>> InputedWarningAsync()
        {
            return await _warningSystemRepository.InputedWarningAsync();
        }
        [RoleDescription("Xem cảnh báo bấp hợp lý nhập xuất")]
        [HttpGet]
        [Route("get-illogical")]
        public async Task<Response<object>> IllogicalDataAsync()
        {
            return await _warningSystemRepository.IllogicalDataAsync();
        }
        [RoleDescription("Xem cảnh báo hệ thống phân phối")]
        [HttpGet]
        [Route("get-distribution-system")]
        public async Task<Response<object>> DistributionSystemDataAsync()
        {
            return await _warningSystemRepository.DistributionSystemDataAsync();
        }
        [RoleDescription("Xem cảnh báo chênh lệch giá bán xăng dầu")]
        [HttpGet]
        [Route("get-deviant-price")]
        public async Task<Response<object>> DeviantPriceDataAsync()
        {
            return await _warningSystemRepository.DeviantPriceDataAsync();
        }
        [RoleDescription("Xem cảnh báo tổng nguồn phân giao của Bộ Công Thương")]
        [HttpGet]
        [Route("get-total-allocations")]
        public async Task<Response<object>> TotalAllocationsDataAsync()
        {
            return await _warningSystemRepository.TotalAllocationsDataAsync();
        }
    }
}
