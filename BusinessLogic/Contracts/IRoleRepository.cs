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
        Task<bool> UpdateListRole(List<Role> roles, IEnumerable<GroupRole> groupRoles, bool isInit = false);
        Task<Guid> UpdateUserRole(UpdateRoleUserDto request);
        Task<GetRolesByUserDtos> GetUserRole(Guid userId);
        Task<GetRolesAndGroupDto> DecentralizatedRole(Guid userId);
        Task<bool> UpdateGroupUserRoleAsync(UpdateGroupRoleUserDto request);
        Task<List<GroupRoleAndRoleDto>> GetListRoleAndGroupsAsync();
        Task<GetRolesByPositionDtos> GetPositionRoleAsync(Guid positionId);
        Task<IEnumerable<Guid?>> GetGetUserCanApproveApi();
        Task<GetRolesByPositionDtos> GetPositionRoleByUserIdAsync(Guid userId);
    }
}
