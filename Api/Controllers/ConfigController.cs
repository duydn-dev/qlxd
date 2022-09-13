using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Api.Attributes;
using BusinessLogic.Contracts;
using Common.Dtos;
using Common.Dtos.LocalityDtos;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Common.Dtos.ConfigDtos;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [UserAuthorize]
    [RoleGroupDescription("Quản lý cấu hình")]
    public class ConfigController : ControllerBase
    {
        private readonly IConfigRepository _configRepository;
        public ConfigController(IConfigRepository configRepository)
        {
            _configRepository = configRepository;
        }

        [RoleDescription("Cấu hình hạn nhập")]
        [HttpPost]
        [Route("save-expire")]
        public async Task<Response<Config>> SaveExpireAsync([FromBody] Config request)
        {
            return await _configRepository.SaveExpireAsync(request);
        }
        
        [AuthenticationOnly]
        [HttpGet]
        [Route("expire-input")]
        public async Task<Response<Config>> GetExpireAsync()
        {
            return await _configRepository.GetExpireAsync();
        }
        
        [AllowAnonymous]
        [HttpGet]
        [Route("get-expire-day")]
        public async Task<Response<GetExpireDayDto>> GetExpireDayAsync()
        {
            return await _configRepository.GetExpireDayAsync();
        }
    }
}
