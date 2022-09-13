using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos.NotificationDtos;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserAuthorize]
    [RoleGroupDescription("Quản lý thông báo")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        public NotificationController(INotificationRepository notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }

        [Route("")]
        [HttpPost]
        [AuthenticationOnly]
        public async Task<object> GetNotificationsAsync([FromBody] GetListNotificationRequestDto request)
        {
            return await _notificationRepository.GetNotificationsAsync(request);
        }

        [Route("remind")]
        [HttpPost]
        [RoleDescription("Gửi nhắc nhở")]
        public async Task<bool> RemindAsync([FromBody] SendRemindDto request)
        {
            return await _notificationRepository.RemindAsync(request);
        }

        [Route("read-all/{userId}")]
        [HttpGet]
        [AuthenticationOnly]
        public async Task<bool> ReadAllAsync(Guid userId)
        {
            return await _notificationRepository.ReadAllAsync(userId);
        }
        [Route("read-one/{id}")]
        [HttpGet]
        [AuthenticationOnly]
        public async Task<bool> ReadOneAsync(long id)
        {
            return await _notificationRepository.ReadOneAsync(id);
        }
    }
}
