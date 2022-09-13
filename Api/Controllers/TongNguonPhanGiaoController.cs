using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.TongNguonPhanGiaoDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserAuthorize]
    [RoleGroupDescription("Quản lý tổng nguồn phân giao")]
    public class TongNguonPhanGiaoController : ControllerBase
    {
        private readonly ITongNguonPhanGiaoRepository _tongNguonPhanGiaoRepository;
        public TongNguonPhanGiaoController(ITongNguonPhanGiaoRepository tongNguonPhanGiaoRepository)
        {
            _tongNguonPhanGiaoRepository = tongNguonPhanGiaoRepository;
        }
        [RoleDescription("Thêm mới tổng nguồn phân giao")]
        [HttpPost]
        [Route("create")]
        public async Task<Response<CreateTongNguonPhanGiaoDto>> CreateAsync([FromBody] CreateTongNguonPhanGiaoDto request)
        {
            return await _tongNguonPhanGiaoRepository.CreateAsync(request);
        }
        [RoleDescription("Cập nhật tổng nguồn phân giao")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<Response<CreateTongNguonPhanGiaoDto>> EditAsync([FromRoute] int id, [FromBody] CreateTongNguonPhanGiaoDto request)
        {
            request.Id = id;
            return await _tongNguonPhanGiaoRepository.EditAsync(request);
        }
        [RoleDescription("Xóa tổng nguồn phân giao")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Response<CreateTongNguonPhanGiaoDto>> DeleteAsync([FromRoute] int id)
        {
            return await _tongNguonPhanGiaoRepository.DeleteAsync(id);
        }
        [RoleDescription("Xem danh sách tổng nguồn phân giao")]
        [HttpPost]
        [Route("get-list")]
        public async Task<Response<GetListResponseModel<List<TongNguonPhanGiaoViewsDto>>>> GetListAsync([FromBody]TongNguongPhanGiaoPageDto request)
        {
            return await _tongNguonPhanGiaoRepository.GetListsync(request);
        }
        [RoleDescription("Xem chi tiết tổng nguồn phân giao")]
        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<Response<TongNguonPhanGiaoViewsDto>> GetByIdAsync([FromRoute] int id)
        {
            return await _tongNguonPhanGiaoRepository.GetByIdAsync(id);
        }
    }
}
