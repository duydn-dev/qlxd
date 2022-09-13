using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.GiaXangDauDoanhNgiepDtos;
using Common.Dtos.GiaXangDauDtos;
using DataAccess;
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
    [RoleGroupDescription("Quản lý giá xăng dầu")]
    public class GiaXangDauController : ControllerBase
    {
        private readonly IGiaXangDauRepository _giaXangDauRepository;
        public GiaXangDauController(IGiaXangDauRepository giaXangDauRepository)
        {
            _giaXangDauRepository = giaXangDauRepository;
        }
        [RoleDescription("Tạo mới bảng giá xăng dầu")]
        [HttpPost]
        [Route("create")]
        public async Task<Response<CreateGiaXangDauDto>> CreateAsync([FromBody]CreateGiaXangDauDto request)
        {
            return await _giaXangDauRepository.CreateAsync(request);
        }
        [RoleDescription("Cập nhật bảng giá xăng dầu")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<Response<CreateGiaXangDauDto>> EditAsync([FromRoute] int id , [FromBody]CreateGiaXangDauDto request)
        {
            request.Id = id;
            return await _giaXangDauRepository.EditAsync(request);
        }
        [RoleDescription("Xóa bảng giá xăng dầu")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Response<CreateGiaXangDauDto>> DeleteAsync(int id)
        {
            return await _giaXangDauRepository.DeleteAsync(id);
        }
        [RoleDescription("Xem chi tiết bảng giá xăng dầu")]
        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<Response<CreateGiaXangDauDto>> GetByIdAsync([FromRoute]int id)
        {
            return await _giaXangDauRepository.GetByIdAsync(id);
        }
        [RoleDescription("Xem danh sách bảng giá xăng dầu")]
        [HttpPost]
        [Route("get-list")]
        public async Task<Response<GetListResponseModel<List<GiaXangDauViewDto>>>> GetGiaXangDauAsync([FromBody] GiaXangDauGetPageDto request)
        {
            return await _giaXangDauRepository.GetGiaXangDauAsync(request);
        }
        [AuthenticationOnly]
        [HttpGet]
        [Route("price-bct")]
        public async Task<Response<List<GiaXangDauDoanhNghiepViewsDto>>> GetPriceBctAsync()
        {
            return await _giaXangDauRepository.GetPriceBctAsync();
        }
        [RoleDescription("Thêm mới giá xăng dầu cho doanh nghiệp")]
        [HttpPost]
        [Route("create-gia-xang-doanh-nghiep")]
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> CreateGiaXangDoanhNghiepAsync([FromBody] CreateGiaXangDauDoanhNghiepDto request)
        {
            return await _giaXangDauRepository.CreateGiaXangDoanhNghiepAsync(request);
        }
        [RoleDescription("Chỉnh sửa giá xăng dầu cho doanh nghiệp")]
        [HttpPut]
        [Route("update-gia-xang-doanh-nghiep/{id}")]
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> EditGiaXangDoanhNghiepAsync([FromRoute] int id,[FromBody]CreateGiaXangDauDoanhNghiepDto request)
        {
            request.Id = id;
            return await _giaXangDauRepository.EditGiaXangDoanhNghiepAsync(request);
        }
        [RoleDescription("Xóa giá xăng dầu cho doanh nghiệp")]
        [HttpDelete]
        [Route("delete-gia-xang-doanh-nghiep/{id}")]
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> DeleteGiaBanDoanhNghiepAsync([FromRoute]int id)
        {
            return await _giaXangDauRepository.DeleteGiaBanDoanhNghiepAsync(id);
        }
        [RoleDescription("Xem danh sách giá xăng dầu cho doanh nghiệp")]
        [HttpGet]
        [Route("get-list-gia-xang-doanh-nghiep")]
        public async Task<Response<List<GiaXangDauDoanhNghiepViewsDto>>> GetListGiaBanDoanhNghiepAsync()
        {
            return await _giaXangDauRepository.GetListGiaBanDoanhNghiepAsync();
        }
        [RoleDescription("Xem chi tiết giá xăng dầu cho doanh nghiệp")]
        [HttpGet]
        [Route("get-by-id-gia-xang-doanh-nghiep/{id}")]
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> GetByIdGiaXangDoanhNghiepAsync(int id)
        {
            return await _giaXangDauRepository.GetByIdGiaXangDoanhNghiepAsync(id);
        }
    }
}
