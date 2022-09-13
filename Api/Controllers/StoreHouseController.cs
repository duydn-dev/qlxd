using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.StoreHouseDtos;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserAuthorize]
    [RoleGroupDescription("Quản lý kho")]
    public class StoreHouseController : ControllerBase
    {
        private readonly IStoreHouseRepository _storeHouseRepository;
        public StoreHouseController(IStoreHouseRepository storeHouseRepository)
        {
            _storeHouseRepository = storeHouseRepository;
        }

        [Route("")]
        [RoleDescription("Xem danh sách kho")]
        [HttpPost]
        public async Task<Response<object>> GetListAsync([FromBody] StoreHouseGetListRequestDto request)
        {
            return await _storeHouseRepository.GetListAsync(request);
        }
        [RoleDescription("Thêm mới Kho")]
        [HttpPost]
        [Route("create")]
        public async Task<Response<CreateStoreHouseDto>> CreateAsync([FromBody] CreateStoreHouseDto request)
        {
            return await _storeHouseRepository.CreateAsync(request);
        }
        [RoleDescription("Chỉnh sửa Kho")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<Response<CreateStoreHouseDto>> EditAsync([FromRoute] int id, [FromBody] CreateStoreHouseDto request)
        {
            request.Id = id;
            return await _storeHouseRepository.EditAsync(request);
        }
        [RoleDescription("Xóa kho")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<Response<CreateStoreHouseDto>> DeleteAsync([FromRoute] int id)
        {
            return await _storeHouseRepository.DeleteAsync(id);
        }
        [RoleDescription("Tìm theo id")]
        [HttpGet]
        [Route("get-by-id/{id}")]
        public async Task<Response<CreateStoreHouseDto>> GetByIdAsync([FromRoute] int id)
        {
            return await _storeHouseRepository.GetByIdAsync(id);
        }
        [RoleDescription("Xem danh mục nhà kho")]
        [HttpGet]
        [Route("get-list-category")]
        public async Task<Response<List<StoreHouseCategory>>> GetListDropDown()
        {
            return await _storeHouseRepository.GetListDropDown();
        }
    }
}
