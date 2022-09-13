using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.PositionDtos;
using Common.Dtos.UserPositionDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý nhóm tài khoản")]
    public class PositionController : ControllerBase
    {
        private readonly IPositionRepository _positionRepository;
        private readonly ILogger<PositionController> _logger;
        public PositionController(IPositionRepository positionRepository, ILogger<PositionController> logger)
        {
            _positionRepository = positionRepository;
            _logger = logger;
        }

        [RoleDescription("Xem danh sách nhóm tài khoản")]
        [Route("")]
        [HttpGet]
        public async Task<Response<GetListResponseModel<List<UserPositionDto>>>> GetUserPositionsAsync([FromQuery]PositionGetFilterDto filter)
        {
            return await _positionRepository.GetUserPositionsAsync(filter);
        }
        
        [Route("{userPositionId}")]
        [AuthenticationOnly]
        [HttpGet]
        public async Task<Response<UserPositionDto>> GetUserPositionsByIdAsync([FromRoute] Guid userPositionId)
        {
            return await _positionRepository.GetUserPositionsByIdAsync(userPositionId);
        }

        [AuthenticationOnly]
        [Route("dropdown")]
        [HttpGet]
        public async Task<IActionResult> GetUserPositionsDropdownAsync()
        {
            try
            {
                return Ok(await _positionRepository.GetUserPositionsDropdownAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu nhóm người dùng, vui lòng thử lại");
            }
        }

        [RoleDescription("Thêm mới nhóm tài khoản")]
        [Route("")]
        [HttpPost]
        public async Task<Response<UserPositionDto>> CreateUserPositionsAsync([FromBody] UserPositionDto request)
        {
            return await _positionRepository.CreateUserPositionsAsync(request);
        }

        [RoleDescription("Chỉnh sửa nhóm tài khoản")]
        [Route("{userPositionId}")]
        [HttpPut]
        public async Task<Response<UserPositionDto>> EditUserPositionsAsync([FromRoute] Guid userPositionId, [FromBody] UserPositionDto request)
        {
            request.UserPositionId = userPositionId;
            return await _positionRepository.EditUserPositionsAsync(request);
        }

        [RoleDescription("Xóa nhóm tài khoản")]
        [Route("{userPositionId}")]
        [HttpDelete]
        public async Task<Response<UserPositionDto>> DeleteUserPositionsAsync([FromRoute] Guid userPositionId)
        {
            return await _positionRepository.DeleteUserPositionsAsync(userPositionId);
        }
    }
}
