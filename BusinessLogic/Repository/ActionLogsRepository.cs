using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.ActionLogs;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class ActionLogsRepository : IActionLogsRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public ActionLogsRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Response<CreateActionLogDto>> CreateAsync(CreateActionLogDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                request.UserId = user.UserId;
                request.TimeLine = DateTime.Now;
                var mapped = _mapper.Map<CreateActionLogDto, ActionLogs>(request);
                await _unitOfWork.GetRepository<ActionLogs>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateActionLogDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateActionLogDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<ActionLogsViewsDto>>>> GetActionLogsAsync(ActionLogGetPageDto request)
        {
            try
            {
                var result = (from al in _unitOfWork.GetRepository<ActionLogs>().GetAll()
                              join u in _unitOfWork.GetRepository<User>().GetAll() on al.UserId equals u.UserId
                              select new ActionLogsViewsDto
                              {
                                  UserName = u.UserName,
                                  ContentLog = al.ContentLog,
                                  TimeLine = al.TimeLine,
                                  Url = al.Url
                              }).WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.UserName.Contains(request.TextSearch))
                                .WhereIf(request.FromDate.HasValue, n => n.TimeLine.Date >= request.FromDate.Value.Date)
                                .WhereIf(request.ToDate.HasValue, n => n.TimeLine.Date <= request.ToDate.Value.Date);
                GetListResponseModel<List<ActionLogsViewsDto>> responseData = new GetListResponseModel<List<ActionLogsViewsDto>>(result.Count(), request.PageSize);
                var data = await result
                    .OrderByDescending(n => n.TimeLine)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();
                responseData.Data = data;
                return Response<GetListResponseModel<List<ActionLogsViewsDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<GetListResponseModel<List<ActionLogsViewsDto>>>.CreateErrorResponse(ex);
            }
        }

    }
}
