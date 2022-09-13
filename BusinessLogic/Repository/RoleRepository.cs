using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common.Dtos;
using Common.Dtos.RoleDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class RoleRepository : IRoleRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public RoleRepository(IUnitOfWork unitOfWork, IUserRepository userRepository, IMemoryCache memoryCache, ILogRepository logRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task<Response<GetRolesAndGroupDto>> DecentralizatedRole(Guid userId)
        {
            try
            {
                var roleSelected = await _unitOfWork.GetRepository<UserRole>().GetAll().Where(n => n.UserId == userId).Select(n => n.RoleId.Value).ToListAsync();

                var listRole = await _unitOfWork.GetRepository<GroupRole>().GetAll().Include(n => n.Roles).Select(n => new DecentralizatedDto
                { 
                    Data = n.GroupRoleId,
                    Label = n.GroupRoleName,
                    Children = n.Roles.Select(g => new DecentralizatedDto { Data = g.RoleId, Label = g.RoleName })
                }).ToListAsync();

                return Response<GetRolesAndGroupDto>.CreateSuccessResponse(new GetRolesAndGroupDto {ListRole = listRole, SelectedIds = roleSelected });
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetRolesAndGroupDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<GetRolesByUserDtos>> GetUserRole(Guid userId)
        {
            try
            {
                var roles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.UserId == userId
                                   select r).Distinct().ToListAsync();

                var userRoles = new GetRolesByUserDtos { UserId = userId, Roles = _mapper.Map<List<Role>,List<RoleDto>>(roles) };
                return Response<GetRolesByUserDtos>.CreateSuccessResponse(userRoles);
            }
            catch(Exception ex)
            {
                return Response<GetRolesByUserDtos>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<bool>> UpdateListRole(List<Role> roles, IEnumerable<GroupRole> groupRoles, bool isInit = false)
        {
            try
            {
                var listRole = _unitOfWork.GetRepository<Role>().GetAll();
                var listGroupRole = _unitOfWork.GetRepository<GroupRole>().GetAll();
                UserCreateDto currentUser = (!isInit) ? await _userRepository.GetIdentityUser() : new UserCreateDto();

                var redundancyRoles = listRole.Where(n => !roles.Select(n => n.RoleCode).Contains(n.RoleCode));
                await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => redundancyRoles.Select(n => n.RoleId).Contains(n.RoleId.Value));
                await _unitOfWork.GetRepository<Role>().DeleteRangeAsync(redundancyRoles);
                // cập nhật lại group user Role
                await AddOrCreateGroupRoleAsync(groupRoles, listGroupRole);

                // cập nhật lại vào danh sách role
                foreach (var role in roles)
                {
                    var roleCode = await listRole.FirstOrDefaultAsync(n => n.RoleCode == role.RoleCode);
                    if (roleCode != null)
                    {
                        roleCode.RoleName = role.RoleName;
                        roleCode.ModifiedBy = currentUser.UserId;
                        roleCode.ModifiedDate = DateTime.Now;
                        roleCode.GroupRoleId = await GetGroupRoleId(role.RoleCode, listGroupRole);
                        await _unitOfWork.GetRepository<Role>().Update(roleCode);
                    }
                    else
                    {
                        
                        role.RoleId = Guid.NewGuid();
                        role.CreatedBy = currentUser.UserId;
                        role.CreatedDate = DateTime.Now;
                        role.GroupRoleId = await GetGroupRoleId(role.RoleCode, listGroupRole);
                        await _unitOfWork.GetRepository<Role>().Add(role);
                    }
                }

                // lấy ra những tài khoản quản trị
                var adminUsers = await (from u in _unitOfWork.GetRepository<User>().GetAll()
                                        join up in _unitOfWork.GetRepository<UserPosition>().GetAll() on u.UserPositionId equals up.UserPositionId
                                        where up.IsAdministrator.Value
                                        select u).ToListAsync();
                // xóa role cũ của admin
                await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => adminUsers.Select(g => g.UserId).Any(g => g == n.UserId));
                await _unitOfWork.SaveAsync();

                // cập nhật role mới
                foreach (var item in adminUsers)
                {
                    var adminRoles = _unitOfWork.GetRepository<Role>().GetAll().Select(n => new UserRole { RoleId = n.RoleId, UserId = item.UserId, UserRoleId = Guid.NewGuid() });
                    await _unitOfWork.GetRepository<UserRole>().AddRangeAsync(adminRoles);
                }

                await _unitOfWork.SaveAsync();

                // update role mới cho quản trị

                return Response<bool>.CreateSuccessResponse(true); ;
            }
            catch(Exception ex)
            {
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<Guid>> UpdateUserRole(UpdateRoleUserDto request)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == request.UserId).Include(n => n.UserRoles).FirstOrDefaultAsync();
                if(user?.UserRoles?.Count > 0)
                {
                    await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => user.UserRoles.Select(g => g.UserId).Any(g => g == n.UserId));
                    await _unitOfWork.SaveAsync();
                }
                foreach (var item in request.RoleIds)
                {
                    await _unitOfWork.GetRepository<UserRole>().Add(new UserRole() { RoleId = item, UserId = request.UserId, UserRoleId = Guid.NewGuid() });
                }
                await _unitOfWork.SaveAsync();

                var roles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.UserId == request.UserId
                                   select r).ToListAsync();
                var userRoles = new GetRolesByUserDtos { UserId = request.UserId, Roles = _mapper.Map<List<Role>, List<RoleDto>>(roles) };
                return Response<Guid>.CreateSuccessResponse(request.UserId);
            }
            catch(Exception ex)
            {
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetRolesByPositionDtos>> GetPositionRoleAsync(Guid positionId)
        {
            try
            {
                var roles = await (from ur in _unitOfWork.GetRepository<GroupRoleUserPosition>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.PositionUserId == positionId
                                   select r).Distinct().ToListAsync();

                var userRoles = new GetRolesByPositionDtos { PositionId = positionId, Roles = _mapper.Map<List<Role>, List<RoleDto>>(roles) };
                return Response<GetRolesByPositionDtos>.CreateSuccessResponse(userRoles);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetRolesByPositionDtos>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> UpdateGroupUserRoleAsync(UpdateGroupRoleUserDto request)
        {
            try
            {
                var userPositions = _unitOfWork.GetRepository<GroupRoleUserPosition>().GetByExpression(n => n.PositionUserId == request.UserPositionId);
                await _unitOfWork.GetRepository<GroupRoleUserPosition>().DeleteRangeAsync(userPositions);

                var roles = await _unitOfWork.GetRepository<Role>().GetByExpression(n => request.RoleIds.Contains(n.RoleId)).ToListAsync();
                var rolesMapped = _mapper.Map<List<Role>, List<RoleDto>>(roles);
                if (request.RoleIds.Count > 0)
                {
                    request.RoleIds.ForEach(async (item) =>
                    {
                        await _unitOfWork.GetRepository<GroupRoleUserPosition>().Add(new GroupRoleUserPosition
                        {
                            GroupRoleUserPositionId = Guid.NewGuid(),
                            PositionUserId = request.UserPositionId,
                            RoleId = item
                        });
                    });
                }
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<GroupRoleAndRoleDto>>> GetListRoleAndGroupsAsync()
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GroupRole>()
                    .GetAll()
                    .OrderBy(n => n.GroupRoleName)
                    .Include(n => n.Roles.OrderBy(n => n.RoleName))
                    .ToListAsync();
                var mapped = _mapper.Map<List<GroupRole>, List<GroupRoleAndRoleDto>>(query);
                return Response<List<GroupRoleAndRoleDto>>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List< GroupRoleAndRoleDto >>.CreateErrorResponse(ex);
            }
        }
        public async Task<IEnumerable<Guid?>> GetGetUserCanApproveApi()
        {
            try
            {
                var userRoles = await (from ur in _unitOfWork.GetRepository<UserRole>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where r.RoleCode.ToLower().Contains("DataManager-ApproveAPI".ToLower())
                                   select ur.UserId).Distinct().ToArrayAsync();

                var positionRoles = await (from ur in _unitOfWork.GetRepository<GroupRoleUserPosition>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where r.RoleCode.ToLower().Contains("DataManager-ApproveAPI".ToLower())
                                   select ur.PositionUserId).Distinct().ToArrayAsync();
                var userInPositions = await _unitOfWork.GetRepository<User>().GetByExpression(n => positionRoles.Contains(n.UserId)).Select(n => (Guid?)n.UserId).ToArrayAsync();

                return userRoles.Union(userInPositions).Distinct();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return new List<Guid?>();
            }
        }

        #region private
        private async Task AddOrCreateGroupRoleAsync(IEnumerable<GroupRole> groupRoles, IQueryable<GroupRole> listGroupRole)
        {
            foreach (var item in groupRoles)
            {
                var groupRoleCode = await listGroupRole.FirstOrDefaultAsync(n => n.GroupRoleCode == item.GroupRoleCode);
                if (groupRoleCode != null)
                {
                    groupRoleCode.GroupRoleName = item.GroupRoleName;
                    await _unitOfWork.GetRepository<GroupRole>().Update(groupRoleCode);
                }
                else
                {

                    item.GroupRoleId = Guid.NewGuid();
                    await _unitOfWork.GetRepository<GroupRole>().Add(item);
                }
            }
            await _unitOfWork.SaveAsync();
        }
        private async Task<Guid> GetGroupRoleId(string roleCode, IQueryable<GroupRole> listGroupRole)
        {
            string roleGroupName = roleCode.Split("-")[0];
            var roleGroup = await listGroupRole.FirstOrDefaultAsync(n => n.GroupRoleCode == roleGroupName);
            return roleGroup.GroupRoleId;
        }

        public async Task<Response<GetRolesByPositionDtos>> GetPositionRoleByUserIdAsync(Guid userId)
        {
            try
            {
                var user = await _unitOfWork.GetAsQueryable<User>().FirstOrDefaultAsync(n => n.UserId == userId);
                var roles = await (from ur in _unitOfWork.GetRepository<GroupRoleUserPosition>().GetAll()
                                   join r in _unitOfWork.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                   where ur.PositionUserId == user.UserPositionId
                                   select r).Distinct().ToListAsync();

                var userRoles = new GetRolesByPositionDtos { PositionId = user.UserPositionId.Value, Roles = _mapper.Map<List<Role>, List<RoleDto>>(roles) };
                return Response<GetRolesByPositionDtos>.CreateSuccessResponse(userRoles);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetRolesByPositionDtos>.CreateErrorResponse(ex);
            }
        }
        #endregion
    }
}
