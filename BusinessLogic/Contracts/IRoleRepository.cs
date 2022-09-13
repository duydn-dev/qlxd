using Common.Dtos;
using Common.Dtos.RoleDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IRoleRepository
    {
        Task<Response<bool>> UpdateListRole(List<Role> roles, IEnumerable<GroupRole> groupRoles, bool isInit = false);
        Task<Response<Guid>> UpdateUserRole(UpdateRoleUserDto request);
        Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId);
        Task<Response<GetRolesAndGroupDto>> DecentralizatedRole(Guid userId);
        Task<Response<bool>> UpdateGroupUserRoleAsync(UpdateGroupRoleUserDto request);
        Task<Response<List<GroupRoleAndRoleDto>>> GetListRoleAndGroupsAsync();
        Task<Response<GetRolesByPositionDtos>> GetPositionRoleAsync(Guid positionId);
        Task<IEnumerable<Guid?>> GetGetUserCanApproveApi();
        Task<Response<GetRolesByPositionDtos>> GetPositionRoleByUserIdAsync(Guid userId);
    }
}
