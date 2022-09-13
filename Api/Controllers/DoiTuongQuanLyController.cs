using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý đối tượng")]
    public class DoiTuongQuanLyController : ControllerBase
    {
        private readonly IDoiTuongQuanLyRepository _doiTuongQuanLyRepository;
        private readonly ILogger<DoiTuongQuanLyController> _logger;
        public DoiTuongQuanLyController(IDoiTuongQuanLyRepository doiTuongQuanLyRepository, ILogger<DoiTuongQuanLyController> logger)
        {
            _doiTuongQuanLyRepository = doiTuongQuanLyRepository;
            _logger = logger;
        }

        [RoleDescription("Thêm mới đối tượng")]
        [HttpPost]
        [Route("create")]
        public async Task<Response<DoiTuongQuanLyViewDto>> CreateAsync([FromBody]DoiTuongQuanLyViewDto request)
        {
            return await _doiTuongQuanLyRepository.CreateAsync(request);
        }
        [RoleDescription("Chỉnh sửa đối tượng")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<Response<DoiTuongQuanLyViewDto>> EditAsync([FromRoute] int id, [FromBody] DoiTuongQuanLyViewDto request)
        {
            request.MaDoiTuong = id;
            return await _doiTuongQuanLyRepository.EditAsync(request);
        }
        [RoleDescription("Xóa đối tượng")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Response<DoiTuongQuanLyViewDto>> DeleteAsync(int id)
        {
            return await _doiTuongQuanLyRepository.DeleteAsync(id);
        }

        [HttpPost]
        [Route("get-list")]
        [RoleDescription("Xem danh sách đối tượng")]
        public async Task<Response<GetListResponseModel<List<DoiTuongQuanLyViewDto>>>> GetQuanLyDoiTuong([FromBody] DoiTuongQuanLyGetPageDto request)
        {
            return await _doiTuongQuanLyRepository.GetQuanLyDoiTuong(request);
        }
        [AuthenticationOnly]
        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<Response<DoiTuongQuanLyViewDto>> GetByIdAsync(int id)
        {
            return await _doiTuongQuanLyRepository.GetByIdAsync(id);
        }
        [AuthenticationOnly]
        [HttpGet]
        [Route("list-doituong")]
        public async Task<IActionResult> ListDoiTuongAsync()
        {
            try
            {
                return Ok(await _doiTuongQuanLyRepository.ListDoiTuongAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu đối tượng quản lý, vui lòng thử lại");
            }
        }

        [AuthenticationOnly]
        [HttpPost]
        [Route("create-child")]
        public async Task<Response<DoiTuongQuanLyViewDto>> CreateChildAsync([FromBody] DoiTuongQuanLyViewDto request)
        {
            return await _doiTuongQuanLyRepository.CreateChildAsync(request);
        }

        [AuthenticationOnly]
        [HttpGet]
        [Route("list-doituong-child")]
        public async Task<Response<List<DoiTuongQuanLy>>> ListDoiTuongConAsync()
        {
            return await _doiTuongQuanLyRepository.ListDoiTuongConAsync();
        }
    }
}
