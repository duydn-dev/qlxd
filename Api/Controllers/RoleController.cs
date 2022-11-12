using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.RoleDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý quyền")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        private readonly ILogger<RoleController> _logger;
        public RoleController(IRoleRepository roleRepository, ILogger<RoleController> logger)
        {
            _roleRepository = roleRepository;
            _logger = logger;
        }

        [AuthenticationOnly]
        [Route("")]
        [HttpGet]
        public async Task<IActionResult> GetListRoleAndGroupsAsync()
        {
            try
            {
                return Ok(await _roleRepository.GetListRoleAndGroupsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu quyền, vui lòng xem lại !");
            }
        }

        [RoleDescription("Cập nhật danh sách quyền")]
        [Route("update-role")]
        [HttpPost]
        public async Task<IActionResult> UpdateRole()
        {
            try
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                var listController = asm.GetTypes()
                    .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                    .Select(n => new GroupRole { GroupRoleId = Guid.Empty, GroupRoleCode = n.Name.Replace("Controller", ""), GroupRoleName = ((RoleGroupDescriptionAttribute)n.GetCustomAttribute(typeof(RoleGroupDescriptionAttribute)))?.Description });

                var controlleractionlist = asm.GetTypes()
                        .Where(type => typeof(ControllerBase).IsAssignableFrom(type))
                        .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                        .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                        .Where(m => m.CustomAttributes.Any(n => n.AttributeType == typeof(RoleDescriptionAttribute)))
                        .Select(x => new Role
                        {
                            RoleCode = x.DeclaringType.Name.Replace("Controller", "") + "-" + x.Name,
                            RoleId = Guid.Empty,
                            RoleName = ((RoleDescriptionAttribute)x.GetCustomAttribute(typeof(RoleDescriptionAttribute)))?.Description
                        })
                        .OrderBy(x => x.RoleCode).ToList();
                return Ok(await _roleRepository.UpdateListRole(controlleractionlist, listController));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể cập nhật dữ liệu quyền, vui lòng xem lại !");
            }
            
        }

        [RoleDescription("Phân quyền tài khoản")]
        [Route("update-user-role")]
        [HttpPost]
        public async Task<IActionResult> UpdateUserRole([FromBody]UpdateRoleUserDto request)
        {
            try
            {
                return Ok(await _roleRepository.UpdateUserRole(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể cập nhật dữ liệu quyền, vui lòng xem lại !");
            }
        }

        [AuthenticationOnly]
        [Route("get-user-role/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetUserRole(Guid userId)
        {
            try
            {
                return Ok(await _roleRepository.GetUserRole(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu quyền, vui lòng xem lại !");
            }
        }

        [AuthenticationOnly]
        [Route("decentralizated-role/{userId}")]
        [HttpGet]
        public async Task<IActionResult> DecentralizatedRole(Guid userId)
        {
            try
            {
                return Ok(await _roleRepository.DecentralizatedRole(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu quyền, vui lòng xem lại !");
            }
        }

        [AuthenticationOnly]
        [Route("get-position-role/{positionId}")]
        [HttpGet]
        public async Task<IActionResult> GetPositionRoleAsync(Guid positionId)
        {
            try
            {
                return Ok(await _roleRepository.GetPositionRoleAsync(positionId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu quyền, vui lòng xem lại !");
            }
        }

        [AuthenticationOnly]
        [Route("get-position-role-by-userId/{userId}")]
        [HttpGet]
        public async Task<IActionResult> GetPositionRoleByUserIdAsync(Guid userId)
        {
            try
            {
                return Ok(await _roleRepository.GetPositionRoleByUserIdAsync(userId));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu quyền, vui lòng xem lại !");
            }
        }


        [RoleDescription("Cập nhật quyền cho vai trò")]
        [Route("decentralizated-group-role")]
        [HttpPost]
        public async Task<IActionResult> UpdateGroupUserRoleAsync([FromBody] UpdateGroupRoleUserDto request)
        {
            try
            {
                return Ok(await _roleRepository.UpdateGroupUserRoleAsync(request));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
                return StatusCode(StatusCodes.Status500InternalServerError, "Không thể lấy dữ liệu quyền, vui lòng xem lại !");
            }
        }
    }
}
