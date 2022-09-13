using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.PositionDtos;
using Common.Dtos.UserPositionDtos;
using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Const.CommonConstant;

namespace BusinessLogic.Repository
{
    public class PositionRepository : IPositionRepository
    {
        private readonly IMapper _mapper;
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        public PositionRepository(ILogRepository logRepository, IUnitOfWork unitOfWork, IMapper mapper, IUserRepository userRepository)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userRepository = userRepository;
        }

        public async Task<Response<PositonGetDropdownViewDto>> CreateAsync(PositonGetDropdownViewDto request)
        {
            try
            {
                request.UserPositionId = Guid.NewGuid();
                var mapped = _mapper.Map<PositonGetDropdownViewDto, UserPosition>(request);
                await _unitOfWork.GetRepository<UserPosition>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<PositonGetDropdownViewDto>.CreateSuccessResponse(request);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<PositonGetDropdownViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<Guid>> DeleteAsync(Guid id)
        {
            try
            {
                var userPosition = await _unitOfWork
                        .GetRepository<UserPosition>()
                        .GetByExpression(n => n.UserPositionId == id)
                        .FirstOrDefaultAsync();

                if(userPosition == null)
                {
                    await _logRepository.ErrorAsync(new Exception($"Chức vụ có id = {id} không tồn tại"));
                    return Response<Guid>.CreateErrorResponse(new Exception($"Chức vụ có id = {id} không tồn tại"));
                }
                await _unitOfWork.GetRepository<UserPosition>().Delete(userPosition);
                await _unitOfWork.SaveAsync();
                return Response<Guid>.CreateSuccessResponse(id);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<Guid>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<GetListResponseModel<List<UserPositionDto>>>> GetUserPositionsAsync(PositionGetFilterDto filter)
        {
            try
            {
                var query = _unitOfWork.GetRepository<UserPosition>()
                                .GetAll()
                                .WhereIf(!string.IsNullOrEmpty(filter.TextSearch), n => n.UserPositionName.Contains(filter.TextSearch));

                GetListResponseModel<List<UserPositionDto>> responseData = new GetListResponseModel<List<UserPositionDto>>(query.Count(), filter.PageSize);
                var result = await query
                    .OrderByDescending(n => n.CreatedDate)
                    .Skip(filter.PageSize * (filter.PageIndex - 1)).Take(filter.PageSize)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<UserPosition>, List<UserPositionDto>>(result);
                return Response<GetListResponseModel<List<UserPositionDto>>>.CreateSuccessResponse(responseData);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<UserPositionDto>>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<UserPositionDto>> CreateUserPositionsAsync(UserPositionDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                request.UserPositionId = Guid.NewGuid();
                request.CreatedBy = currentUser?.UserId;
                request.CreatedDate = DateTime.Now;
                var mapped = _mapper.Map<UserPositionDto, UserPosition>(request);
                await _unitOfWork.GetRepository<UserPosition>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<UserPositionDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<UserPositionDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<UserPositionDto>> GetUserPositionsByIdAsync(Guid userPositionId)
        {
            try
            {
                var userPosition = await _unitOfWork
                    .GetRepository<UserPosition>()
                    .GetByExpression(n => n.UserPositionId == userPositionId)
                    .FirstOrDefaultAsync();

                var mapped = _mapper.Map<UserPosition, UserPositionDto>(userPosition);
                return Response<UserPositionDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<UserPositionDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<UserPositionDto>> DeleteUserPositionsAsync(Guid userPositionId)
        {
            try
            {
                var userPosition = await _unitOfWork
                    .GetRepository<UserPosition>()
                    .GetByExpression(n => n.UserPositionId == userPositionId)
                    .FirstOrDefaultAsync();

                await _unitOfWork.GetRepository<UserPosition>().Delete(userPosition);
                await _unitOfWork.SaveAsync();
                var mapped = _mapper.Map<UserPosition, UserPositionDto>(userPosition);
                return Response<UserPositionDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<UserPositionDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<UserPositionDto>> EditUserPositionsAsync(UserPositionDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var position = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.UserPositionId == request.UserPositionId);
                request.ModifiedBy = currentUser?.UserId;
                request.ModifiedDate = DateTime.Now;
                request.IsAdministrator = position.IsAdministrator;
                var mapped = _mapper.Map<UserPositionDto, UserPosition>(request, position);
                await _unitOfWork.GetRepository<UserPosition>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<UserPositionDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<UserPositionDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<List<PositonGetDropdownViewDto>> GetUserPositionsDropdownAsync()
        {
            var data = await _unitOfWork.GetRepository<UserPosition>().GetAll().Where(n => n.Status == (int)UserPositionStatusEnum.Active).ToListAsync();
            return _mapper.Map<List<UserPosition>, List<PositonGetDropdownViewDto>>(data);
        }

        public async Task<Response<PositonGetDropdownViewDto>> UpdateAsync(PositonGetDropdownViewDto request)
        {
            try
            {
                var userPosition = await _unitOfWork
                        .GetRepository<UserPosition>()
                        .GetByExpression(n => n.UserPositionId == request.UserPositionId)
                        .FirstOrDefaultAsync();

                if (userPosition == null)
                {
                    await _logRepository.ErrorAsync(new Exception($"Chức vụ có id = {request.UserPositionId} không tồn tại"));
                    return Response<PositonGetDropdownViewDto>.CreateErrorResponse(new Exception($"Chức vụ có id = {request.UserPositionId} không tồn tại"));
                }
                var mapped = _mapper.Map<PositonGetDropdownViewDto, UserPosition>(request);
                await _unitOfWork.GetRepository<UserPosition>().Update(userPosition);
                await _unitOfWork.SaveAsync();
                return Response<PositonGetDropdownViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<PositonGetDropdownViewDto>.CreateErrorResponse(ex);
            }
        }
    }
}
