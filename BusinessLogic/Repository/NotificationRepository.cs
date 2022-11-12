using BusinessLogic.Contracts;
using BusinessLogic.Hubs;
using BusinessLogic.UnitOfWork;
using Common.Dtos.NotificationDtos;
using DataAccess;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly IHubContext<AppHub> _hubContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserRepository _userRepository;
        private readonly ILogRepository _logRepository;

        public NotificationRepository(IHubContext<AppHub> hubContext, IUnitOfWork unitOfWork, IUserRepository userRepository, ILogRepository logRepository)
        {
            _hubContext = hubContext;
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _logRepository = logRepository;
        }
        public async Task SendNotification(Notification notification)
        {
            await _unitOfWork.GetRepository<Notification>().Add(notification);
            await _unitOfWork.SaveAsync();
            await _hubContext.Clients.All.SendAsync("UpdateNumberNotifi", notification);
        }
        public async Task SendNotificationByLocalityId(Notification notification, int localityId)
        {
            var users = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.LocalityId == localityId).ToListAsync();
            var lstNotify = new List<Notification>();
            foreach (var item in users)
            {
                lstNotify.Add(new Notification
                {
                    Content = notification.Content,
                    CreatedBy = notification.CreatedBy,
                    CreatedDate = notification.CreatedDate,
                    IsReaded = notification.IsReaded,
                    Url = notification.Url,
                    RecipientId = item.UserId,
                    Title = notification.Title,
                });
            }
            await _unitOfWork.GetRepository<Notification>().AddRangeAsync(lstNotify);
            await _unitOfWork.SaveAsync();
            await _hubContext.Clients.All.SendAsync("UpdateNumberNotifiByLocalityId", lstNotify);
        }
        public async Task SendNotificationToListUser(Notification notification, IEnumerable<Guid?> userIds)
        {
            var lstNotify = new List<Notification>();
            foreach (var item in userIds)
            {
                lstNotify.Add(new Notification
                {
                    Content = notification.Content,
                    CreatedBy = notification.CreatedBy,
                    CreatedDate = notification.CreatedDate,
                    IsReaded = notification.IsReaded,
                    Url = notification.Url,
                    RecipientId = item,
                    Title = notification.Title,
                });
            }
            await _unitOfWork.GetRepository<Notification>().AddRangeAsync(lstNotify);
            await _unitOfWork.SaveAsync();
            await _hubContext.Clients.All.SendAsync("SendNotificationToListUser", lstNotify);
        }
        public async Task<object> GetNotificationsAsync(GetListNotificationRequestDto request)
        {
            try
            {
                var data = from u in _unitOfWork.GetRepository<User>().GetAll()
                           join n in _unitOfWork.GetRepository<Notification>().GetAll() on u.UserId equals n.CreatedBy
                           select new
                           {
                               u.FullName,
                               u.Avatar,
                               n.Title,
                               n.Content,
                               n.CreatedBy,
                               n.CreatedDate,
                               n.IsReaded,
                               n.RecipientId,
                               n.Url,
                               n.Id,
                               n.Files
                           };
                return await data
                    .Where(n => n.RecipientId == request.UserId)
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize * (request.PageIndex - 1))
                    .Take(request.PageSize)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                return new List<Notification>();
            }
        }

        public async Task<bool> RemindAsync(SendRemindDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.MaDoiTuong == request.MaDoiTuong).ToListAsync();
                var lstNotify = new List<Notification>();
                foreach (var item in user)
                {
                    lstNotify.Add(new Notification
                    {
                        Content = request.Content,
                        CreatedBy = currentUser.UserId,
                        CreatedDate = DateTime.Now,
                        IsReaded = request.IsReaded,
                        RecipientId = item.UserId,
                        Title = request.Title,
                        Files = request.Files
                    });
                }
                await _unitOfWork.GetRepository<Notification>().AddRangeAsync(lstNotify);
                await _unitOfWork.SaveAsync();
                await _hubContext.Clients.All.SendAsync("Remind", lstNotify);
                return true;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return false;
            }
        }

        public async Task<bool> ReadAllAsync(Guid userId)
        {
            try
            {
                var notis = await _unitOfWork.GetRepository<Notification>().GetByExpression(n => n.RecipientId == userId && !n.IsReaded.Value).ToListAsync();
                notis.ForEach(async n =>
                {
                    n.IsReaded = true;
                    await _unitOfWork.GetRepository<Notification>().Update(n);
                });
                
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return false;
            }
        }

        public async Task<bool> ReadOneAsync(long id)
        {
            try
            {
                var notis = await _unitOfWork.GetAsQueryable<Notification>().FirstOrDefaultAsync(n => n.Id == id);
                notis.IsReaded = true;
                await _unitOfWork.GetRepository<Notification>().Update(notis);
                await _unitOfWork.SaveAsync();
                return true;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return false;
            }
        }
    }
}
