using Common.Dtos;
using Common.Dtos.PositionDtos;
using Common.Dtos.UserPositionDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IPositionRepository
    {
        Task<Response<GetListResponseModel<List<UserPositionDto>>>> GetUserPositionsAsync(PositionGetFilterDto filter);
        Task<List<PositonGetDropdownViewDto>> GetUserPositionsDropdownAsync();
        Task<Response<PositonGetDropdownViewDto>> CreateAsync(PositonGetDropdownViewDto request);
        Task<Response<PositonGetDropdownViewDto>> UpdateAsync(PositonGetDropdownViewDto request);
        Task<Response<Guid>> DeleteAsync(Guid id);
        Task<Response<UserPositionDto>> GetUserPositionsByIdAsync(Guid userPositionId);
        Task<Response<UserPositionDto>> CreateUserPositionsAsync(UserPositionDto request);
        Task<Response<UserPositionDto>> EditUserPositionsAsync(UserPositionDto request);
        Task<Response<UserPositionDto>> DeleteUserPositionsAsync(Guid userPositionId);
    }
}
