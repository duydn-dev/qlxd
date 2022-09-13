using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.DepartmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserAuthorize]
    [RoleGroupDescription("Quản lý phòng ban")]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;
        public DepartmentController(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        [HttpGet]
        [RoleDescription("Xem danh sách phòng ban")]
        public async Task<Response<GetListResponseModel<List<DepartmentViewDto>>>> GetListDepartmentAsync([FromQuery] DepartmentGetPageDto request)
        {
            return await _departmentRepository.GetDepartments(request);
        }

        [HttpGet]
        [Route("{id}")]
        [RoleDescription("Chi tiết phòng ban")]
        public async Task<Response<DepartmentViewDto>> GetDepartmentAsync(Guid id)
        {
            return await _departmentRepository.GetByIdAsync(id);
        }

        [HttpPost]
        [RoleDescription("Tạo phòng ban")]
        public async Task<Response<DepartmentViewDto>> CreateAsync([FromBody] DepartmentViewDto request)
        {
            return await _departmentRepository.CreateAsync(request);
        }

        [HttpPut]
        [Route("{id}")]
        [RoleDescription("Chỉnh sửa phòng ban")]
        public async Task<Response<DepartmentViewDto>> EditAsync([FromRoute] Guid id, [FromBody] DepartmentViewDto request)
        {
            request.DepartmentId = id;
            return await _departmentRepository.EditAsync(request);
        }

        [HttpDelete]
        [Route("{id}")]
        [RoleDescription("Xóa phòng ban")]
        public async Task<Response<DepartmentViewDto>> DeleteAsync(Guid id)
        {
            return await _departmentRepository.DeleteAsync(id);
        }
    }
}
