using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;
using Common.Dtos;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Common.Const;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Api.Attributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class UserAuthorizeAttribute : Attribute, IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            AttachUserToContext(context.HttpContext, token);
            var allowAttr = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(AllowAnonymousAttribute), false).FirstOrDefault();
            if (allowAttr != null)
            {
                await next();
                return;
            }
            var authenOnly = (context.ActionDescriptor as ControllerActionDescriptor).MethodInfo.GetCustomAttributes(typeof(AuthenticationOnlyAttribute), false).FirstOrDefault();
            if (context.HttpContext.Items["User"] == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    ContentType = "application/json",
                    Content = "Bạn chưa đăng nhập !"
                };
                return;
            }
            if (authenOnly != null && context.HttpContext.Items["User"] == null)
            {
                context.Result = new ContentResult()
                {
                    StatusCode = 401,
                    ContentType = "application/json",
                    Content = "Bạn chưa đăng nhập !"
                };
                return;
            }
            else if (authenOnly != null && context.HttpContext.Items["User"] != null)
            {
                await next();
                return;
            }
            else
            {
                UserCreateDto userCreateDto = (UserCreateDto)context.HttpContext.Items["User"];
                var cacheService = context.HttpContext.RequestServices.GetRequiredService<IMemoryCache>();
                var userService = context.HttpContext.RequestServices.GetRequiredService<IUserRepository>();
                var unitOfWorkService = context.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();
                var responseUser = await userService.GetUserByUserName(userCreateDto.UserName);
                // là admin thì cho qua
                if (responseUser.ResponseData.UserPosition.IsAdministrator.HasValue && responseUser.ResponseData.UserPosition.IsAdministrator.Value)
                {
                    await next();
                }
                else
                {
                    // không phải admin thì check quyền
                    var roles = (from u in unitOfWorkService.GetRepository<User>().GetAll()
                                 join ur in unitOfWorkService.GetRepository<UserRole>().GetAll() on u.UserId equals ur.UserId
                                 join r in unitOfWorkService.GetRepository<Role>().GetAll() on ur.RoleId equals r.RoleId
                                 where u.UserName == userCreateDto.UserName
                                 select r.RoleCode);

                    var position = await unitOfWorkService.GetRepository<User>()
                        .GetAll()
                        .FirstOrDefaultAsync(n => n.UserName == userCreateDto.UserName);

                    var postionRoles = from up in unitOfWorkService.GetRepository<UserPosition>().GetAll()
                                       join upr in unitOfWorkService.GetRepository<GroupRoleUserPosition>().GetAll() on up.UserPositionId equals upr.PositionUserId
                                       join r in unitOfWorkService.GetRepository<Role>().GetAll() on upr.RoleId equals r.RoleId
                                       where up.UserPositionId == position.UserPositionId
                                       select r.RoleCode;

                    //var roles = responseUser.ResponseData?.UserRoles?.Select(n => n?.Role?.RoleCode);

                    string controllerName = context.ActionDescriptor.RouteValues["controller"].ToString();
                    string actionName = context.ActionDescriptor.RouteValues["action"].ToString();

                    //var identityRoles = context.HttpContext.User.Claims.Select(n => n.Value);
                    if (!roles.Contains(controllerName + "-" + actionName) && !postionRoles.Contains(controllerName + "-" + actionName))
                    {
                        context.Result = new ContentResult()
                        {
                            StatusCode = 401,
                            ContentType = "application/json",
                            Content = JsonConvert.SerializeObject(new { responseData = "bạn không có quyền vào trang này !" })
                        };
                        return;
                    }
                    await next();
                }
            }
        }
        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    context.Items["User"] = null;
                }
                // lấy ra public key
                var webHostEnvironment = context.RequestServices.GetRequiredService<IWebHostEnvironment>();
                var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();
                var publicKey = Convert.FromBase64String(CommonConstant.PublicKey);
                using RSA rsa = RSA.Create();
                rsa.ImportRSAPublicKey(publicKey, out _);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings["JWT:ValidIssuer"],
                    ValidAudience = appSettings["JWT:ValidAudience"],
                    IssuerSigningKey = new RsaSecurityKey(rsa),

                    CryptoProviderFactory = new CryptoProviderFactory()
                    {
                        CacheSignatureProviders = false
                    }
                };
                var handler = new JwtSecurityTokenHandler();
                handler.ValidateToken(token, validationParameters, out var validatedSecurityToken);
                var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

                var claims = jwtToken.Claims;
                UserCreateDto userInfo = new UserCreateDto();
                userInfo.UserId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.UserId)).Value);
                userInfo.UserName = claims.FirstOrDefault(n => n.Type == ClaimTypes.Name).Value;
                userInfo.Email = claims.FirstOrDefault(n => n.Type == nameof(userInfo.Email))?.Value;
                userInfo.FullName = claims.FirstOrDefault(n => n.Type == nameof(userInfo.FullName))?.Value;
                userInfo.NumberPhone = claims.FirstOrDefault(n => n.Type == nameof(userInfo.FullName))?.Value;
                userInfo.Avatar = claims.FirstOrDefault(n => n.Type == nameof(userInfo.FullName))?.Value;
                userInfo.Address = claims.FirstOrDefault(n => n.Type == nameof(userInfo.Address))?.Value;
                userInfo.Status = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Status)).Value);
                userInfo.UserPositionId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.UserPositionId)).Value);
                userInfo.DepartmentId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.DepartmentId)).Value);
                userInfo.LocalityId = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.LocalityId)).Value);
                userInfo.LocalityLevel = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.LocalityLevel)).Value);
                userInfo.Province = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Province)).Value);
                userInfo.District = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.District)).Value);
                userInfo.Ward = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Ward)).Value);
                userInfo.Province = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Province)).Value);
                userInfo.IsAdministrator = Convert.ToBoolean(claims.FirstOrDefault(n => n.Type == nameof(userInfo.IsAdministrator)).Value);

                context.Items["User"] = userInfo;
            }
            catch
            {
                context.Items["User"] = null;
            }
        }
    }
}
