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

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý quyền")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;
        public RoleController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        [AuthenticationOnly]
        [Route("")]
        [HttpGet]
        public async Task<Response<List<GroupRoleAndRoleDto>>> GetListRoleAndGroupsAsync()
        {
            return await _roleRepository.GetListRoleAndGroupsAsync();
        }

        [RoleDescription("Cập nhật danh sách quyền")]
        [Route("update-role")]
        [HttpPost]
        public async Task<Response<bool>> UpdateRole()
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
            return await _roleRepository.UpdateListRole(controlleractionlist, listController);
        }

        [RoleDescription("Phân quyền tài khoản")]
        [Route("update-user-role")]
        [HttpPost]
        public async Task<Response<Guid>> UpdateUserRole([FromBody]UpdateRoleUserDto request)
        {
            return await _roleRepository.UpdateUserRole(request);
        }

        [AuthenticationOnly]
        [Route("get-user-role/{userId}")]
        [HttpGet]
        public async Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId)
        {
            return await _roleRepository.GetUserRole(userId);
        }

        [AuthenticationOnly]
        [Route("decentralizated-role/{userId}")]
        [HttpGet]
        public async Task<Response<GetRolesAndGroupDto>> DecentralizatedRole(Guid userId)
        {
            return await _roleRepository.DecentralizatedRole(userId);
        }

        [AuthenticationOnly]
        [Route("get-position-role/{positionId}")]
        [HttpGet]
        public async Task<Response<GetRolesByPositionDtos>> GetPositionRoleAsync(Guid positionId)
        {
            return await _roleRepository.GetPositionRoleAsync(positionId);
        }

        [AuthenticationOnly]
        [Route("get-position-role-by-userId/{userId}")]
        [HttpGet]
        public async Task<Response<GetRolesByPositionDtos>> GetPositionRoleByUserIdAsync(Guid userId)
        {
            return await _roleRepository.GetPositionRoleByUserIdAsync(userId);
        }


        [RoleDescription("Cập nhật quyền cho vai trò")]
        [Route("decentralizated-group-role")]
        [HttpPost]
        public async Task<Response<bool>> UpdateGroupUserRoleAsync([FromBody] UpdateGroupRoleUserDto request)
        {
            return await _roleRepository.UpdateGroupUserRoleAsync(request);
        }
    }
}
