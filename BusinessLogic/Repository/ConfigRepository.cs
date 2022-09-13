using Microsoft.EntityFrameworkCore;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.LocalityDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Dtos.ConfigDtos;

namespace BusinessLogic.Repository
{
    public class ConfigRepository : IConfigRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ConfigRepository(ILogRepository logRepository, IUnitOfWork unitOfWork)
        {
            _logRepository = logRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Response<DataAccess.Config>> SaveExpireAsync(DataAccess.Config request)
        {
            try
            {
                var config = await _unitOfWork.GetRepository<DataAccess.Config>().GetAll().FirstOrDefaultAsync(n => n.ConfigId == request.ConfigId);
                config.ConfigValue = request.ConfigValue;
                config.ConfigName = request.ConfigName;
                await _unitOfWork.GetRepository<DataAccess.Config>().Update(config);
                await _unitOfWork.SaveAsync();
                return Response<DataAccess.Config>.CreateSuccessResponse(config);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DataAccess.Config>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<DataAccess.Config>> GetExpireAsync()
        {
            try
            {
                var config = await _unitOfWork.GetRepository<DataAccess.Config>().GetAll().FirstOrDefaultAsync(n => n.ConfigKey == "expire-input");
                return Response<DataAccess.Config>.CreateSuccessResponse(config);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DataAccess.Config>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<GetExpireDayDto>> GetExpireDayAsync()
        {
            try
            {
                var config = await _unitOfWork.GetRepository<DataAccess.Config>().GetAll().FirstOrDefaultAsync(n => n.ConfigKey == "expire-input");
                int expireDateNumber = Convert.ToInt32(config.ConfigValue);
                DateTime baseDate = DateTime.Today;
                var thisWeekStart = baseDate.AddDays(-(int)baseDate.DayOfWeek).AddDays(1);
                var thisWeekEnd = thisWeekStart.AddDays(7).AddSeconds(-1);
                var prevWeek = thisWeekStart.AddDays(-7);
                var prevWeekStart = prevWeek.AddDays(-(int)prevWeek.DayOfWeek).AddDays(1);
                var prevWeekEnd = prevWeekStart.AddDays(7).AddSeconds(-1);
                for (DateTime date = thisWeekStart; date <= thisWeekEnd; date = date.AddDays(1))
                {
                    if ((int)date.DayOfWeek == (expireDateNumber - 1))
                    {
                        return Response<GetExpireDayDto>.CreateSuccessResponse(new GetExpireDayDto
                        {
                            ToDay = baseDate,
                            ThisWeekStart = thisWeekStart,
                            ThisWeekEnd = thisWeekEnd,
                            ThisExpireDate = date,
                            PrevWeekStart = prevWeekStart,
                            PrevWeekEnd = prevWeekEnd,
                            ThisMonthStart = new DateTime(baseDate.Year, baseDate.Month, 1),
                            ThisMonthEnd = new DateTime(baseDate.Year, baseDate.Month ,DateTime.DaysInMonth(baseDate.Year, baseDate.Month)),
                            PrevMonthStart = new DateTime(baseDate.Year, baseDate.Month, 1).AddMonths(-1),
                            PrevMonthEnd = new DateTime(baseDate.Year, baseDate.Month, DateTime.DaysInMonth(baseDate.Year, baseDate.Month)).AddMonths(-1),
                        });
                    }
                }

                return Response<GetExpireDayDto>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetExpireDayDto>.CreateErrorResponse(ex);
            }
        }
    }
}
