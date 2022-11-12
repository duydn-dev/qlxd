using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Const;
using Common.Dtos;
using Common.Dtos.UserDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;
using Google.Authenticator;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography;

namespace BusinessLogic.Repository
{
    [Obsolete]
    public class UserRepository : IUserRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IWarningSystemRepository _warningSystemRepository;
        private readonly TwoFactorAuthenticator _twoFactorAuthenticator;
        public UserRepository(
            ILogRepository logRepository,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IHostingEnvironment hostingEnvironment,
            IWarningSystemRepository warningSystemRepository,
            TwoFactorAuthenticator twoFactorAuthenticator
            )
        {
            _logRepository = logRepository;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _hostingEnvironment = hostingEnvironment;
            _twoFactorAuthenticator = twoFactorAuthenticator;
            _warningSystemRepository = warningSystemRepository;
        }

        #region CRUD
        public async Task<Response<GetListResponseModel<List<ListUserResponseDto>>>> GetListUser(GetListUserRequestDto request)
        {
            var query = _unitOfWork.GetRepository<User>().GetAll();
            query = query.WhereIf(!string.IsNullOrEmpty(request.TextSearch),
                n => n.UserName.Contains(request.TextSearch) ||
                     n.FullName.Contains(request.TextSearch) ||
                     n.Address.Contains(request.TextSearch) ||
                     n.Email.Contains(request.TextSearch) ||
                     n.NumberPhone.Contains(request.TextSearch)
                );
            query = query.WhereIf(request.Status.HasValue, n => n.Status == request.Status);

            var result = (from q in query
                          join p in _unitOfWork.GetRepository<UserPosition>().GetAll() on q.UserPositionId equals p.UserPositionId
                          select new ListUserResponseDto
                          {
                              UserId = q.UserId,
                              UserName = q.UserName,
                              FullName = q.FullName,
                              Email = q.Email,
                              NumberPhone = q.NumberPhone,
                              Avatar = q.Avatar,
                              Address = q.Address,
                              CreatedBy = q.CreatedBy,
                              CreatedDate = q.CreatedDate,
                              ModifiedBy = q.ModifiedBy,
                              ModifiedDate = q.ModifiedDate,
                              Status = q.Status,
                              UserPositionId = p.UserPositionId,
                              UserPositionName = p.UserPositionName,
                              IsAdministrator = p.IsAdministrator,
                              District = q.District,
                              LocalityId = q.LocalityId,
                              LocalityLevel = q.LocalityLevel,
                              Province = q.Province,
                              Ward = q.Ward,
                              CreatedCode = q.CreatedCode,
                          });

            GetListResponseModel<List<ListUserResponseDto>> responseData = new GetListResponseModel<List<ListUserResponseDto>>(result.Count(), request.PageSize);
            var data = await result
                .OrderByDescending(n => n.CreatedDate)
                .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                .ToListAsync();
            responseData.Data = data;
            return Response<GetListResponseModel<List<ListUserResponseDto>>>.CreateSuccessResponse(responseData);
        }
        public async Task<Response<User>> GetUserByUserId(Guid userId)
        { 
            var user = await _unitOfWork.GetAsQueryable<User>().AsNoTracking().FirstOrDefaultAsync(n => n.UserId == userId);
            if (user == null) return new Response<User>(false, StatusCodes.Status404NotFound, "Không tìm thấy tài khoản !", null);
            user.PassWord = "";
            return new Response<User>(true, StatusCodes.Status200OK, "Lấy dữ liệu thành công !", user);
        }
        public async Task<UserCreateDto> Create(UserCreateDto request)
        {
            request.CreatedBy = (request.UserId == Guid.Empty) ? (await GetIdentityUser()).UserId : request.UserId;
            request.UserId = Guid.NewGuid();
            request.CreatedDate = DateTime.Now;
            request.PassWord = Md5Encrypt.MD5Hash(request.PassWord);

            var setupCode = _twoFactorAuthenticator.GenerateSetupCode(CommonConstant.Issuer, CommonConstant.AppName, request.UserName, false);
            var userMapped = _mapper.Map<UserCreateDto, User>(request);
            userMapped.CreatedCode = setupCode.ManualEntryKey;
            await _unitOfWork.GetRepository<User>().Add(userMapped);
            await _unitOfWork.SaveAsync();
            return request;
        }

        public async Task<bool> Delete(Guid userId)
        {
            await _unitOfWork.GetRepository<UserRole>().DeleteByExpression(n => n.UserId == userId);
            await _unitOfWork.GetRepository<User>().DeleteByExpression(n => n.UserId == userId);
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<bool> DeleteMany(List<Guid> userIds)
        {
            await _unitOfWork.GetRepository<User>().DeleteByExpression(n => userIds.Contains(n.UserId));
            await _unitOfWork.SaveAsync();
            return true;
        }

        public async Task<Response<UserCreateDto>> Update(UserCreateDto request)
        {
            var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == request.UserId).FirstOrDefaultAsync();
            if (user == null) return new Response<UserCreateDto>(false, StatusCodes.Status404NotFound, "Không tìm thấy tài khoản này", null);
            var setupCode = _twoFactorAuthenticator.GenerateSetupCode(CommonConstant.Issuer, CommonConstant.AppName, request.UserName, false);
            request.PassWord = user.PassWord;
            var userMapped = _mapper.Map<UserCreateDto, User>(request, user);
            userMapped.CreatedCode = setupCode.ManualEntryKey;
            await _unitOfWork.SaveAsync();
            return Response<UserCreateDto>.CreateSuccessResponse(request);
        }
        #endregion
        public async Task<Response<User>> GetUserByUserName(string userName)
        {
            try
            {
                return Response<User>.CreateSuccessResponse(await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserName == userName).Include(n => n.UserRoles).Include(n => n.UserPosition).FirstOrDefaultAsync());
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<User>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<UserLoginResponseDto>> Login(UserLoginDto request)
        {
            var responseUser = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserName == request.UserName).Include(n => n.UserPosition).FirstOrDefaultAsync();
            var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
            if (responseUser == null)
            {
                return new Response<UserLoginResponseDto>(false, StatusCodes.Status404NotFound, "Không tìm thấy tài khoản này !", null);
            }
            if (responseUser.Status == UserStatus.Locked)
            {
                return new Response<UserLoginResponseDto>(false, StatusCodes.Status423Locked, "Tài khoản đang bị khóa !", null);
            }
            if (Md5Encrypt.MD5Hash(request.PassWord) != responseUser.PassWord)
            {
                return new Response<UserLoginResponseDto>(false, StatusCodes.Status500InternalServerError, "Sai mật khẩu, vui lòng xem lại", null);
            }
            string createdCode = responseUser.CreatedCode;
            if (Convert.ToBoolean(_configuration.GetSection("isUseGoogleAuthen").Value))
            {
                var isValidCode = _twoFactorAuthenticator.ValidateTwoFactorPIN(request.UserName, request.GoogleAuthenCode);
                if (!isValidCode)
                {
                    return new Response<UserLoginResponseDto>(false, StatusCodes.Status500InternalServerError, "Sai mã xác thực hoặc đã hết hạn, vui lòng xem lại", null);
                }
            }
            var token = await GenerateToken(responseUser);
            await _unitOfWork.GetRepository<ActionLogs>().Add(new ActionLogs
            {
                ContentLog = "Tài khoản " + request.UserName + " đã đăng nhập vào lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                UserId = responseUser.UserId,
                TimeLine = DateTime.Now,
            });
            await _unitOfWork.SaveAsync();
            return Response<UserLoginResponseDto>.CreateSuccessResponse(new UserLoginResponseDto()
            {
                Address = responseUser.Address,
                Email = responseUser.Email,
                UserName = responseUser.UserName,
                FullName = responseUser.FullName,
                NumberPhone = responseUser.NumberPhone,
                UserId = responseUser.UserId,
                Token = token.token,
                Expire = token.expire,
                Avatar = responseUser.Avatar,
                LocalityId = responseUser.LocalityId,
                LocalityLevel = responseUser.LocalityLevel,
                IsAdministrator = responseUser.UserPosition?.IsAdministrator,
                UserPositionId = responseUser.UserPositionId,
                IsBCT = responseUser.IsBCT
            });
        }
        private async Task<dynamic> GenerateToken(User user)
        {
            DateTime expire = DateTime.Now.AddHours(Convert.ToInt32(_configuration["JWT:expireHour"]));
            var privateKey = Convert.FromBase64String(CommonConstant.PrivateKey);

            using RSA rsa = RSA.Create();
            rsa.ImportRSAPrivateKey(privateKey, out _);

            var signingCredentials = new SigningCredentials(new RsaSecurityKey(rsa), SecurityAlgorithms.RsaSha256)
            {
                CryptoProviderFactory = new CryptoProviderFactory { CacheSignatureProviders = false }
            };

            var now = DateTime.Now;
            var unixTimeSeconds = new DateTimeOffset(now).ToUnixTimeSeconds();

            var jwt = new JwtSecurityToken(
                audience: _configuration["JWT:ValidAudience"],
                issuer: _configuration["JWT:ValidIssuer"],
                claims: new Claim[] {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.Email ?? ""),
                    new Claim("FullName", user.FullName ?? ""),
                    new Claim("NumberPhone", user.NumberPhone ?? ""),
                    new Claim("Avatar", user.Avatar ?? ""),
                    new Claim("Address", user.Address ?? ""),
                    new Claim("Status", (user.Status ?? 0).ToString()),
                    new Claim("UserPositionId", (user.UserPositionId ?? Guid.Empty).ToString()),
                    new Claim("DepartmentId", (user.DepartmentId ?? Guid.Empty).ToString()),
                    new Claim("LocalityId", (user.LocalityId ?? 0).ToString()),
                    new Claim("LocalityLevel",(user.LocalityLevel ?? 0).ToString()),
                    new Claim("Province", (user.Province ?? 0).ToString()),
                    new Claim("District",(user.District ?? 0).ToString()),
                    new Claim("Ward",(user.Ward ?? 0).ToString()),
                    new Claim("IsBCT", user.IsBCT.ToString()),
                    new Claim("IsAdministrator", (user.UserPosition?.IsAdministrator ?? false).ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                },
                notBefore: now,
                expires: expire,
                signingCredentials: signingCredentials
            );

            return new { expire, token = new JwtSecurityTokenHandler().WriteToken(jwt) };
        }
        public async Task<UserCreateDto> GetIdentityUser()
        {
            //var claims = _httpContextAccessor.HttpContext.User.Claims;
            //UserCreateDto userInfo = new UserCreateDto();
            //userInfo.UserId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.UserId)).Value);
            //userInfo.UserName = claims.FirstOrDefault(n => n.Type == ClaimTypes.Name).Value;
            //userInfo.Email = claims.FirstOrDefault(n => n.Type == nameof(userInfo.Email))?.Value;
            //userInfo.FullName = claims.FirstOrDefault(n => n.Type == nameof(userInfo.FullName))?.Value;
            //userInfo.NumberPhone = claims.FirstOrDefault(n => n.Type == nameof(userInfo.FullName))?.Value;
            //userInfo.Avatar = claims.FirstOrDefault(n => n.Type == nameof(userInfo.FullName))?.Value;
            //userInfo.Address = claims.FirstOrDefault(n => n.Type == nameof(userInfo.Address))?.Value;
            //userInfo.Status = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Status)).Value);
            //userInfo.UserPositionId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.UserPositionId)).Value);
            //userInfo.DepartmentId = new Guid(claims.FirstOrDefault(n => n.Type == nameof(userInfo.DepartmentId)).Value);
            //userInfo.LocalityId = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.LocalityId)).Value);
            //userInfo.LocalityLevel = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.LocalityLevel)).Value);
            //userInfo.Province = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Province)).Value);
            //userInfo.District = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.District)).Value);
            //userInfo.Ward = Convert.ToInt32(claims.FirstOrDefault(n => n.Type == nameof(userInfo.Ward)).Value);
            //userInfo.IsBCT = Convert.ToBoolean(claims.FirstOrDefault(n => n.Type == nameof(userInfo.IsBCT)).Value);
            //return userInfo;
            return (UserCreateDto)_httpContextAccessor.HttpContext.Items["User"];
        }

        public async Task<Response<string>> UploadAvatar(IFormFile avatar)
        {
            try
            {
                string newFileName = Path.GetFileNameWithoutExtension(avatar.FileName) + DateTime.Now.ToString("ddMMyyyyHHmmss") + Path.GetExtension(avatar.FileName);
                string filePath = "Uploads\\" + newFileName;
                string vitualPath = Path.Combine(_hostingEnvironment.WebRootPath, filePath);
                using (var stream = new FileStream(vitualPath, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }
                return Response<string>.CreateSuccessResponse(filePath);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<string>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<UserProfileViewDto>> GetUserProfileAsync(Guid userId)
        {
            try
            {
                var userProfile = await (from u in _unitOfWork.GetRepository<User>().GetAll()

                                         join up in _unitOfWork.GetRepository<UserPosition>().GetAll() on u.UserPositionId equals up.UserPositionId
                                         join dep in _unitOfWork.GetRepository<Department>().GetAll() on u.DepartmentId equals dep.DepartmentId
                                         into grDep
                                         from grDepData in grDep.DefaultIfEmpty()
                                         where u.UserId == userId
                                         select new UserProfileViewDto
                                         {
                                             Address = u.Address,
                                             Avatar = u.Avatar,
                                             CreatedBy = u.CreatedBy,
                                             CreatedDate = u.CreatedDate,
                                             DepartmentId = u.DepartmentId,
                                             DepartmentName = up.UserPositionName,
                                             Email = u.Email,
                                             FullName = u.FullName,
                                             ModifiedBy = u.ModifiedBy,
                                             ModifiedDate = u.ModifiedDate,
                                             NumberPhone = u.NumberPhone,
                                             PassWord = u.PassWord,
                                             PositionName = grDepData != null ? grDepData.DepartmentName : "",
                                             Status = u.Status,
                                             UserId = u.UserId,
                                             UserName = u.UserName,
                                             UserPositionId = u.UserPositionId
                                         }).FirstOrDefaultAsync();
                return Response<UserProfileViewDto>.CreateSuccessResponse(userProfile);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<UserProfileViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<bool>> UpdateAvatarAsync(IFormFile avatar, Guid userId)
        {
            try
            {
                var file = await UploadAvatar(avatar);
                var user = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == userId).FirstOrDefaultAsync();
                user.Avatar = file.ResponseData;
                await _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveAsync();
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<bool>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<object>> ChangePasswordAsync(Guid id, ChangePassWordDto request)
        {
            try
            {
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == id && n.PassWord == Md5Encrypt.MD5Hash(request.Password));
                if (user == null)
                    return Response<object>.CreateErrorResponse("Sai mật khẩu hoặc không tồn tại tài khoản muốn đổi, vui lòng xem lại !");

                user.PassWord = Md5Encrypt.MD5Hash(request.NewPassword);
                await _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveAsync();
                return Response<object>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<object>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<Locality>>> GetByLocalityAsync(int type, int parenId)
        {
            try
            {
                var display = await _unitOfWork.GetRepository<Locality>().GetByExpression(x => x.Type == type && x.ParentId == parenId).ToListAsync();
                return Response<List<Locality>>.CreateSuccessResponse(display);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<List<Locality>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<UserLoginResponseDto>> GetToken(UserLoginDto request)
        {
            var responseUser = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserName == request.UserName).Include(n => n.UserPosition).FirstOrDefaultAsync();
            var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
            if (responseUser == null)
            {
                return new Response<UserLoginResponseDto>(false, StatusCodes.Status404NotFound, "Không tìm thấy tài khoản này !", null);
            }
            if (responseUser.Status == UserStatus.Locked)
            {
                return new Response<UserLoginResponseDto>(false, StatusCodes.Status423Locked, "Tài khoản đang bị khóa !", null);
            }
            if (Md5Encrypt.MD5Hash(request.PassWord) != responseUser.PassWord)
            {
                return new Response<UserLoginResponseDto>(false, StatusCodes.Status500InternalServerError, "Sai mật khẩu, vui lòng xem lại", null);
            }
            var token = await GenerateToken(responseUser);
            await _unitOfWork.GetRepository<ActionLogs>().Add(new ActionLogs
            {
                ContentLog = "Tài khoản " + request.UserName + " đã đăng nhập vào lúc " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"),
                UserId = responseUser.UserId,
                TimeLine = DateTime.Now,
            });
            await _unitOfWork.SaveAsync();
            return Response<UserLoginResponseDto>.CreateSuccessResponse(new UserLoginResponseDto()
            {
                Address = responseUser.Address,
                Email = responseUser.Email,
                UserName = responseUser.UserName,
                FullName = responseUser.FullName,
                NumberPhone = responseUser.NumberPhone,
                UserId = responseUser.UserId,
                Token = token.token,
                Expire = token.expire,
                Avatar = responseUser.Avatar,
                LocalityId = responseUser.LocalityId,
                LocalityLevel = responseUser.LocalityLevel,
                IsAdministrator = responseUser.UserPosition?.IsAdministrator,
                UserPositionId = responseUser.UserPositionId,
                IsBCT = responseUser.IsBCT
            });
        }
        public async Task<Response<object>> ForgotPasswordAsync(Guid id)
        {
            try
            {
                var user = await _unitOfWork.GetAsQueryable<User>().FirstOrDefaultAsync(n => n.UserId == id);
                if (user == null)
                    return Response<object>.CreateErrorResponse("Không tồn tại tài khoản muốn đổi mật khẩu, vui lòng xem lại !");

                string newPass = "1Qaz2wsx";
                string hashPass = Md5Encrypt.MD5Hash(newPass);
                user.PassWord = hashPass;

                await _unitOfWork.GetRepository<ActionLogs>().Add(new ActionLogs { ContentLog = $"{user.FullName} - Id: {user.UserId} đã cập nhật mật khẩu vào {DateTime.Now.ToString("dd/MM/yyyy HH:mm")}", TimeLine = DateTime.Now, Url = "", UserId = user.UserId });
                await _unitOfWork.GetRepository<User>().Update(user);
                await _unitOfWork.SaveAsync();
                return Response<object>.CreateSuccessResponse(newPass);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<object>.CreateErrorResponse("Không thể gửi mail, vui lòng kiểm tra lại email và thử lại !");
            }
        }
        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}