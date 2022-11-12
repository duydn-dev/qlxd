using Microsoft.AspNetCore.Http;
using Common.Dtos;
using Common.Dtos.UserDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IUserRepository
    {
        Task<Response<UserLoginResponseDto>> Login(UserLoginDto request);
        Task<Response<User>> GetUserByUserName(string userName);
        Task<Response<User>> GetUserByUserId(Guid userId);
        Task<Response<UserProfileViewDto>> GetUserProfileAsync(Guid userId);
        Task<Response<GetListResponseModel<List<ListUserResponseDto>>>> GetListUser(GetListUserRequestDto request);
        Task<UserCreateDto> Create(UserCreateDto request);
        Task<Response<UserCreateDto>> Update(UserCreateDto request);
        Task<bool> Delete(Guid userId);
        Task<bool> DeleteMany(List<Guid> userId);
        Task<UserCreateDto> GetIdentityUser();
        Task<Response<string>> UploadAvatar(IFormFile avatar);
        Task<Response<bool>> UpdateAvatarAsync(IFormFile avatar, Guid userId);
        Task<Response<object>> ChangePasswordAsync(Guid id, ChangePassWordDto request);
        Task<Response<List<Locality>>> GetByLocalityAsync(int type, int parenId);
        Task<Response<UserLoginResponseDto>> GetToken(UserLoginDto request);
        Task<Response<object>> ForgotPasswordAsync(Guid id);
    }
}
