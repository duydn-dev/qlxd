using Common.Dtos.NotificationDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface INotificationRepository
    {
        Task SendNotification(Notification notification);
        Task SendNotificationByLocalityId(Notification notification, int localityId);
        Task<object> GetNotificationsAsync(GetListNotificationRequestDto request);
        Task SendNotificationToListUser(Notification notification, IEnumerable<Guid?> userIds);
        Task<bool> RemindAsync(SendRemindDto request);
        Task<bool> ReadAllAsync(Guid userId);
        Task<bool> ReadOneAsync(long id);
    }
}
