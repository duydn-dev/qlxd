using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Const;
using Common.Dtos;
using Common.Dtos.CreateApiDtos;
using Common.Dtos.DataManagerDtos;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static Common.Const.CommonConstant;

namespace BusinessLogic.Repository
{
    public class DataManagerRepository : IDataManagerRepository
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;
        private readonly IActionLogsRepository _actionLogsRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IConfigRepository _configRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DataManagerRepository(IMapper mapper, IUnitOfWork unitOfWork, ILogRepository logRepository, IUserRepository userRepository
            , IConfiguration configuration, IActionLogsRepository actionLogsRepository, INotificationRepository notificationRepository, IConfigRepository configRepository, IRoleRepository roleRepository, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _userRepository = userRepository;
            _configuration = configuration;
            _actionLogsRepository = actionLogsRepository;
            _notificationRepository = notificationRepository;
            _configRepository = configRepository;
            _roleRepository = roleRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        #region Hiển thị API
        public async Task<Response<GetListResponseModel<List<ListApiDataResponseDto>>>> GetFilterApi1Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var data = _unitOfWork.GetRepository<GiayDangKyKinhDoanh>()
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenDoanhNghiep.ToLower().Contains(request.TextSearch.ToLower()))
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.ThoiGianCapNhat.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.ThoiGianCapNhat.Value.Date <= request.ToDate.Value.Date);


                GetListResponseModel<List<ListApiDataResponseDto>> responseData = new GetListResponseModel<List<ListApiDataResponseDto>>(await data.CountAsync(), request.PageSize.Value);
                var result = await data
                    .OrderByDescending(n => n.ThoiGianCapNhat)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1))
                    .Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = result.Select(n => new ListApiDataResponseDto
                {
                    Id = n.Id,
                    DiaChi = n.DiaChi,
                    ThoiGianCapNhat = n.ThoiGianCapNhat,
                    TenDoanhNghiep = n.TenDoanhNghiep,
                    SoDkkd = n.SoDkkd,
                    SoDienThoai = n.SoDienThoai,
                    DiaChiHuyen = n.DiaChiHuyen,
                    DiaChiTinh = n.DiaChiTinh,
                    DiaChiXa = n.DiaChiXa,
                    MaDoiTuong = n.MaDoiTuong,
                    MaSoThue = n.MaSoThue,
                    NguoiDaiDienCccd = n.NguoiDaiDienCccd,
                    NguoiDaiDienDob = n.NguoiDaiDienDob,
                    NguoiDaiDienSdt = n.NguoiDaiDienSdt,
                    NguoiDaiDienTen = n.NguoiDaiDienTen,
                    MaPhienBan = n.MaPhienBan,
                    AprrovedStatus = n.AprrovedStatus,
                    Version = n.Version,
                    AprrovedBy = n.AprrovedBy,
                    RejectMessage = n.RejectMessage,
                    AprrovedDate = n.AprrovedDate,
                }).ToList();
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                    item.TenTinh = (await _unitOfWork.GetRepository<Locality>().GetAll().FirstOrDefaultAsync(n => n.Id == item.DiaChiTinh))?.Title;
                    item.TenHuyen = (await _unitOfWork.GetRepository<Locality>().GetAll().FirstOrDefaultAsync(n => n.Id == item.DiaChiHuyen))?.Title;
                    item.TenXa = (await _unitOfWork.GetRepository<Locality>().GetAll().FirstOrDefaultAsync(n => n.Id == item.DiaChiXa))?.Title;
                }
                return Response<GetListResponseModel<List<ListApiDataResponseDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<ListApiDataResponseDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<HeThongPhanPhoiDto>>>> GetFilterApi2Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var data = _unitOfWork.GetRepository<HeThongPhanPhoi>()
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenHeThongPhanPhoi.ToLower().Contains(request.TextSearch.ToLower()))
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuongCha == user.MaDoiTuong)
                    .WhereIf(request.FromDate.HasValue, n => n.ThoiGianCapNhat.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.ThoiGianCapNhat.Value.Date <= request.ToDate.Value.Date)
                    .WhereIf(request.Status.HasValue, n => n.IsActive == request.Status);

                GetListResponseModel<List<HeThongPhanPhoiDto>> responseData = new GetListResponseModel<List<HeThongPhanPhoiDto>>(await data.CountAsync(), request.PageSize.Value);
                responseData.Data = _mapper.Map<List<HeThongPhanPhoi>, List<HeThongPhanPhoiDto>>(await data.OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value).ToListAsync());
                foreach (var item in responseData.Data)
                {
                    item.DoiTuongCha = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuongCha))?.TenDoiTuong;
                    item.DoiTuongCon = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuongCon))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<HeThongPhanPhoiDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<HeThongPhanPhoiDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<SanXuatDto>>>> GetFilterApi3Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<SanXuat>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<SanXuatDto>> responseData = new GetListResponseModel<List<SanXuatDto>>(await data.CountAsync(), request.PageSize.Value);
                responseData.Data = _mapper.Map<List<SanXuat>, List<SanXuatDto>>(await data.OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value).ToListAsync());
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<SanXuatDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<SanXuatDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<TieuThuDto>>>> GetFilterApi4Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<TieuThu>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);
                GetListResponseModel<List<TieuThuDto>> responseData = new GetListResponseModel<List<TieuThuDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();

                responseData.Data = _mapper.Map<List<TieuThu>, List<TieuThuDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<TieuThuDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<TieuThuDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<NhapDto>>>> GetFilterApi5Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<Nhap>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<NhapDto>> responseData = new GetListResponseModel<List<NhapDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<Nhap>, List<NhapDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<NhapDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<NhapDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<TonKhoDto>>>> GetFilterApi6Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<TonKho>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<TonKhoDto>> responseData = new GetListResponseModel<List<TonKhoDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<TonKho>, List<TonKhoDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<TonKhoDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<TonKhoDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<NguyenLieuDto>>>> GetFilterApi7Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<NguyenLieu>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<NguyenLieuDto>> responseData = new GetListResponseModel<List<NguyenLieuDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<NguyenLieu>, List<NguyenLieuDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<NguyenLieuDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<NguyenLieuDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<KhacDto>>>> GetFilterApi8Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<Khac>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<KhacDto>> responseData = new GetListResponseModel<List<KhacDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<Khac>, List<KhacDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<KhacDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<KhacDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<QuyBinhOnGiaDto>>>> GetFilterApi9Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<QuyBinhOnGium>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                    .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<QuyBinhOnGiaDto>> responseData = new GetListResponseModel<List<QuyBinhOnGiaDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<QuyBinhOnGium>, List<QuyBinhOnGiaDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<QuyBinhOnGiaDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<QuyBinhOnGiaDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<DuKienNhapDto>>>> GetFilterApi10Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<DuKienNhap>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = query
                    .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                    .WhereIf(request.FromDate.HasValue, n => n.CreatedDate.Value.Date >= request.FromDate.Value.Date)
                    .WhereIf(request.ToDate.HasValue, n => n.CreatedDate.Value.Date <= request.ToDate.Value.Date);

                GetListResponseModel<List<DuKienNhapDto>> responseData = new GetListResponseModel<List<DuKienNhapDto>>(await data.CountAsync(), request.PageSize.Value);
                var lstResut = await data
                    .OrderByDescending(n => n.Id)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<DuKienNhap>, List<DuKienNhapDto>>(lstResut);
                foreach (var item in responseData.Data)
                {
                    item.TenDoiTuong = (await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().FirstOrDefaultAsync(n => n.MaDoiTuong == item.MaDoiTuong))?.TenDoiTuong;
                }
                return Response<GetListResponseModel<List<DuKienNhapDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<DuKienNhapDto>>>.CreateErrorResponse(ex);
            }
        }
        #endregion

        #region GetById
        public async Task<Response<object>> GetById(GetByIdRequestDto request)
        {
            try
            {
                switch (request.Type)
                {
                    case (int)ApiNumberEnum.Api1:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api2:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<HeThongPhanPhoi>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api3:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<SanXuat>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api4:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<TieuThu>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api5:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<Nhap>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api6:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<TonKho>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api7:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<NguyenLieu>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api8:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<Khac>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api9:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<QuyBinhOnGium>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    case (int)ApiNumberEnum.Api10:
                        return Response<object>.CreateSuccessResponse(await _unitOfWork.GetRepository<DuKienNhap>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id));
                    default:
                        break;
                }
                return Response<object>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {

                await _logRepository.ErrorAsync(ex);
                return Response<object>.CreateErrorResponse(ex);
            }
        }
        #endregion

        #region Tạo mới API
        public async Task<Response<CreateGiayDangKyKinhdoanhDto>> CreateApi1Async(CreateGiayDangKyKinhdoanhDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<GiayDangKyKinhDoanh>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<GiayDangKyKinhDoanh>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.ThoiGianCapNhat = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.ThoiGianCapNhat.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateGiayDangKyKinhdoanhDto, GiayDangKyKinhDoanh>(request);
                        await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu giấy đăng ký kinh doanh vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api1", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu giấy đăng ký kinh doanh vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu giấy đăng ký kinh doanh vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api1"
                        }, usersCanApproved);
                        return Response<CreateGiayDangKyKinhdoanhDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateHeThongPhanPhoiDto>> CreateApi2Async(CreateHeThongPhanPhoiDto request)
        {
            try
            {
                request.ThoiGianCapNhat = DateTime.Now;
                request.IsActive = true;
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                request.MaDoiTuongCha = user.MaDoiTuong;
                var check = await _unitOfWork.GetRepository<HeThongPhanPhoi>().GetByExpression(x => x.MaDoiTuongCha == request.MaDoiTuongCha && x.MaDoiTuongCon == request.MaDoiTuongCon).AnyAsync();
                if (!check)
                {
                    var mapped = _mapper.Map<CreateHeThongPhanPhoiDto, HeThongPhanPhoi>(request);
                    await _unitOfWork.GetRepository<HeThongPhanPhoi>().Add(mapped);
                    await _unitOfWork.SaveAsync();
                    await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới hệ thống phân phối vào {request.ThoiGianCapNhat.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.ThoiGianCapNhat.Value, Url = "/api2", UserId = currentUser.UserId });
                    return Response<CreateHeThongPhanPhoiDto>.CreateSuccessResponse(request);
                }
                return Response<CreateHeThongPhanPhoiDto>.CreateErrorResponse("Đã tồn tại đối tượng này, vui lòng xem lại !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateHeThongPhanPhoiDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateSanXuatDto>> CreateApi3Async(CreateSanXuatDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<SanXuat>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<SanXuat>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateSanXuatDto, SanXuat>(request);
                        await _unitOfWork.GetRepository<SanXuat>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu sản xuất vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api3", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu sản xuất vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu giấy sản xuất vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api3"
                        }, usersCanApproved);
                        return Response<CreateSanXuatDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateSanXuatDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateSanXuatDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateSanXuatDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateTieuThuDto>> CreateApi4Async(CreateTieuThuDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<TieuThu>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<TieuThu>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateTieuThuDto, TieuThu>(request);
                        var added = await _unitOfWork.GetRepository<TieuThu>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu tiêu thụ vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api4", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu tiêu thụ vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu tiêu thụ vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api4"
                        }, usersCanApproved);
                        return Response<CreateTieuThuDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateTieuThuDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateTieuThuDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTieuThuDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateNhapDto>> CreateApi5Async(CreateNhapDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<Nhap>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<Nhap>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateNhapDto, Nhap>(request);
                        await _unitOfWork.GetRepository<Nhap>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu nhập vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api5", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu nhập vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu nhập vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api5"
                        }, usersCanApproved);
                        return Response<CreateNhapDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateNhapDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateNhapDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateNhapDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateTonKhoDto>> CreateApi6Async(CreateTonKhoDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<TonKho>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<TonKho>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateTonKhoDto, TonKho>(request);
                        await _unitOfWork.GetRepository<TonKho>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu tồn kho vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api6", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu tồn kho vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu tồn kho vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api6"
                        }, usersCanApproved);
                        return Response<CreateTonKhoDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateTonKhoDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateTonKhoDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTonKhoDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateNguyenLieuDto>> CreateApi7Async(CreateNguyenLieuDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<NguyenLieu>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<NguyenLieu>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateNguyenLieuDto, NguyenLieu>(request);
                        await _unitOfWork.GetRepository<NguyenLieu>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu nguyên liệu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api7", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu nguyên liệu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu nguyên liệu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api7"
                        }, usersCanApproved);
                        return Response<CreateNguyenLieuDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateNguyenLieuDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateNguyenLieuDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateNguyenLieuDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateKhacDto>> CreateApi8Async(CreateKhacDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<Khac>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<Khac>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateKhacDto, Khac>(request);
                        await _unitOfWork.GetRepository<Khac>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu báo cáo khác cho thương nhân đầu mối xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api8", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu báo cáo khác cho thương nhân đầu mối xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu báo cáo khác cho thương nhân đầu mối xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api8"
                        }, usersCanApproved);
                        return Response<CreateKhacDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateKhacDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateKhacDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateKhacDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateQuyBinhOnGiumDto>> CreateApi9Async(CreateQuyBinhOnGiumDto request)
        {
            try
            {
                int max = 0;
                if (await _unitOfWork.GetAsQueryable<QuyBinhOnGium>().AnyAsync())
                {
                    max = await _unitOfWork.GetAsQueryable<QuyBinhOnGium>().MaxAsync(x => x.MaPhienBan);
                }
                request.MaPhienBan = max + 1;
                request.Version = 1;
                request.IsDelete = false;
                request.CreatedDate = DateTime.Now;
                request.NgayBc = DateTime.Now.AddDays(-7).Date;
                var exprireDate = await _configRepository.GetExpireAsync();
                if (exprireDate.Success)
                {
                    int reportDate = (int)request.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate != 1 && reportDate <= Convert.ToInt32(exprireDate.ResponseData.ConfigValue))
                    {
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateQuyBinhOnGiumDto, QuyBinhOnGium>(request);
                        await _unitOfWork.GetRepository<QuyBinhOnGium>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu qũy bình ổn giá vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api9", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu qũy bình ổn giá vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu qũy bình ổn giá vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api9"
                        }, usersCanApproved);
                        return Response<CreateQuyBinhOnGiumDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateDuKienNhapDto>> CreateApi10Async(CreateDuKienNhapDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                request.MaDoiTuong = user.MaDoiTuong;
                request.CreatedDate = DateTime.Now;
                request.NgayBC = DateTime.Now;
                request.CreatedBy = currentUser.UserId;
                var mapped = _mapper.Map<CreateDuKienNhapDto, DuKienNhap>(request);
                await _unitOfWork.GetRepository<DuKienNhap>().Add(mapped);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu qũy bình ổn giá vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api10", UserId = currentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu dự kiến nhập vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = currentUser.UserId,
                    CreatedDate = request.CreatedDate,
                    IsReaded = false,
                    Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã thêm mới số liệu dự kiến nhập vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api10"
                }, usersCanApproved);
                return Response<CreateDuKienNhapDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateDuKienNhapDto>.CreateErrorResponse(ex);
            }
        }
        #endregion

        public async Task<Response<List<ChungLoaiDto>>> ListChungLoaiAsync()
        {
            try
            {
                var query = from cl in _unitOfWork.GetAsQueryable<ChungLoai>()
                            join ch in _unitOfWork.GetAsQueryable<CauHinh>() on cl.MaChungLoai equals ch.MaChungLoai
                            select new ChungLoaiDto
                            {
                                MaChungLoai = cl.MaChungLoai,
                                TenChungLoai = cl.TenChungLoai,
                                NgayTao = cl.NgayTao,
                                MaDonViTinh = ch.DonViTinh,
                                TenDonViTinh = ch.TenDonVi
                            };
                var ListData = await query.ToListAsync();
                return Response<List<ChungLoaiDto>>.CreateSuccessResponse(ListData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<List<ChungLoaiDto>>.CreateErrorResponse(ex);
            }
        }

        #region Sửa Api
        public async Task<Response<CreateGiayDangKyKinhdoanhDto>> UpdateApi1Async(CreateGiayDangKyKinhdoanhDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }

                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.ThoiGianCapNhat.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>()
                        .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                        .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateGiayDangKyKinhdoanhDto, GiayDangKyKinhDoanh>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu giấy đăng ký kinh doanh vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api1", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu giấy đăng ký kinh doanh vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu giấy đăng ký kinh doanh vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api1"
                        }, usersCanApproved);
                        return Response<CreateGiayDangKyKinhdoanhDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiayDangKyKinhdoanhDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateHeThongPhanPhoiDto>> UpdateApi2Async(CreateHeThongPhanPhoiDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var query = await _unitOfWork.GetRepository<HeThongPhanPhoi>()
                    .GetByExpression(n => n.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateHeThongPhanPhoiDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }
                request.ThoiGianCapNhat = DateTime.Now;
                var mapped = _mapper.Map<CreateHeThongPhanPhoiDto, HeThongPhanPhoi>(request, query);
                await _unitOfWork.GetRepository<HeThongPhanPhoi>().Update(mapped);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật hệ thống phân phối vào {request.ThoiGianCapNhat.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.ThoiGianCapNhat.Value, Url = "/api2", UserId = currentUser.UserId });
                return Response<CreateHeThongPhanPhoiDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateHeThongPhanPhoiDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateSanXuatDto>> UpdateApi3Async(CreateSanXuatDto request)
        {
            try
            {

                var updateData = await _unitOfWork.GetRepository<SanXuat>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateSanXuatDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }
                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<SanXuat>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<SanXuat>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        var mapped = _mapper.Map<CreateSanXuatDto, SanXuat>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<SanXuat>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu sản xuất vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api3", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình sản xuất xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình sản xuất xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api3"
                        }, usersCanApproved);
                        return Response<CreateSanXuatDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateSanXuatDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateSanXuatDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateSanXuatDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateTieuThuDto>> UpdateApi4Async(CreateTieuThuDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<TieuThu>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateTieuThuDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }
                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<TieuThu>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<TieuThu>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateTieuThuDto, TieuThu>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<TieuThu>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu tiêu thụ vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api4", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình tiêu thụ vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình tiêu thụ vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api4"
                        }, usersCanApproved);
                        return Response<CreateTieuThuDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateTieuThuDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateTieuThuDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTieuThuDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateNhapDto>> UpdateApi5Async(CreateNhapDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<Nhap>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateNhapDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }

                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {

                        var query = await _unitOfWork.GetRepository<Nhap>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<Nhap>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var mapped = _mapper.Map<CreateNhapDto, Nhap>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<Nhap>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu nhập vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api5", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình nhập xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình nhập xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api5"
                        }, usersCanApproved);
                        return Response<CreateNhapDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateNhapDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateNhapDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateNhapDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateTonKhoDto>> UpdateApi6Async(CreateTonKhoDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<TonKho>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateTonKhoDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }

                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<TonKho>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<TonKho>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        var mapped = _mapper.Map<CreateTonKhoDto, TonKho>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<TonKho>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu tồn kho vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api6", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình tồn kho vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Tình hình tồn kho vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api6"
                        }, usersCanApproved);
                        return Response<CreateTonKhoDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateTonKhoDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateTonKhoDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTonKhoDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateNguyenLieuDto>> UpdateApi7Async(CreateNguyenLieuDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<NguyenLieu>()
               .GetByExpression(n => n.Id == request.Id)
               .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateNguyenLieuDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }

                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<NguyenLieu>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<NguyenLieu>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        var mapped = _mapper.Map<CreateNguyenLieuDto, NguyenLieu>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<NguyenLieu>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu nguyên liệu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api7", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Lượng nguyên liệu nhập khẩu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Lượng nguyên liệu nhập khẩu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api7"
                        }, usersCanApproved);
                        return Response<CreateNguyenLieuDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateNguyenLieuDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateNguyenLieuDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateNguyenLieuDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateKhacDto>> UpdateApi8Async(CreateKhacDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<Khac>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateKhacDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }

                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<Khac>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<Khac>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        var mapped = _mapper.Map<CreateKhacDto, Khac>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<Khac>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu báo cáo khác vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api8", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Báo cáo khác cho TMĐMKD vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Báo cáo khác cho TNĐMKD vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api8"
                        }, usersCanApproved);
                        return Response<CreateKhacDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateKhacDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateKhacDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateKhacDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateQuyBinhOnGiumDto>> UpdateApi9Async(CreateQuyBinhOnGiumDto request)
        {
            try
            {
                var updateData = await _unitOfWork.GetRepository<QuyBinhOnGium>()
                .GetByExpression(n => n.Id == request.Id)
                .FirstOrDefaultAsync();
                if (updateData == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }

                var expireDate = await _configRepository.GetExpireAsync();
                if (expireDate.Success)
                {
                    int reportDate = (int)updateData.NgayBc.Value.DayOfWeek + 1;
                    if (reportDate <= Convert.ToInt32(expireDate.ResponseData.ConfigValue))
                    {
                        var query = await _unitOfWork.GetRepository<QuyBinhOnGium>()
                    .GetByExpression(n => n.MaPhienBan == request.MaPhienBan)
                    .FirstOrDefaultAsync();
                        var max = await _unitOfWork.GetRepository<QuyBinhOnGium>().GetAll().Where(x => x.MaPhienBan == request.MaPhienBan).MaxAsync(x => x.Version);
                        request.Version = max + 1;
                        request.CreatedDate = DateTime.Now;
                        request.AprrovedDate = null;
                        request.AprrovedStatus = 0;
                        request.AprrovedBy = null;
                        request.RejectMessage = null;
                        request.MD5 = null;
                        var currentUser = await _userRepository.GetIdentityUser();
                        var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                        request.MaDoiTuong = user.MaDoiTuong;
                        var mapped = _mapper.Map<CreateQuyBinhOnGiumDto, QuyBinhOnGium>(request);
                        mapped.Id = 0;
                        await _unitOfWork.GetRepository<QuyBinhOnGium>().Add(mapped);
                        await _unitOfWork.SaveAsync();
                        await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.CreatedDate.Value, Url = "/api9", UserId = currentUser.UserId });
                        var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                        await _notificationRepository.SendNotificationToListUser(new Notification
                        {
                            Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Quỹ bình ổn giá xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            CreatedBy = currentUser.UserId,
                            CreatedDate = request.CreatedDate,
                            IsReaded = false,
                            Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Quỹ bình ổn giá xăng dầu vào {request.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                            Url = "/api9"
                        }, usersCanApproved);
                        return Response<CreateQuyBinhOnGiumDto>.CreateSuccessResponse(request);
                    }
                    await _logRepository.ErrorAsync("Đã hết thời hạn nhập !");
                    return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse("Đã hết thời hạn nhập !");
                }
                await _logRepository.ErrorAsync("Chưa cấu hình thời hạn nhập !");
                return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse("Chưa cấu hình thời hạn nhập !");
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateQuyBinhOnGiumDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateDuKienNhapDto>> UpdateApi10Async(CreateDuKienNhapDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var query = await _unitOfWork.GetRepository<DuKienNhap>()
                    .GetByExpression(n => n.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    await _logRepository.ErrorAsync("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                    return Response<CreateDuKienNhapDto>.CreateErrorResponse("Không tồn tại bản ghi muốn cập nhập, vui lòng xem lại !");
                }
                request.ModifiedDate = DateTime.Now;
                request.ModifiedBy = currentUser.UserId;
                var mapped = _mapper.Map<CreateDuKienNhapDto, DuKienNhap>(request, query);
                await _unitOfWork.GetRepository<DuKienNhap>().Update(mapped);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật hệ thống phân phối vào {request.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = request.ModifiedDate.Value, Url = "/api10", UserId = currentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Dự kiến nhập kỳ tới vào {request.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = currentUser.UserId,
                    CreatedDate = request.ModifiedDate,
                    IsReaded = false,
                    Title = $"{currentUser.FullName} - Id: {currentUser.UserId} đã cập nhật số liệu Dự kiến nhập kỳ tới vào {request.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api10"
                }, usersCanApproved);
                return Response<CreateDuKienNhapDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateDuKienNhapDto>.CreateErrorResponse(ex);
            }
        }
        #endregion

        #region Xác nhận API
        public async Task<Response<bool>> ApproveAPI1(CreateGiayDangKyKinhdoanhDto giayDangKyKinhDoanh)
        {
            try
            {
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                var authClaims = new List<Claim>
                {
                    new Claim("MaDoiTuong", giayDangKyKinhDoanh.MaDoiTuong.ToString()),
                    new Claim("TenDoanhNghiep", giayDangKyKinhDoanh.TenDoanhNghiep ?? ""),
                    new Claim("SoDkkd", giayDangKyKinhDoanh.SoDkkd ?? ""),
                    new Claim("MaSoThue", giayDangKyKinhDoanh.MaSoThue ?? ""),
                    new Claim("DiaChi", giayDangKyKinhDoanh.DiaChi ?? ""),
                    new Claim("DiaChiXa",  (giayDangKyKinhDoanh.DiaChiXa ?? 0).ToString()),
                    new Claim("DiaChiHuyen",  (giayDangKyKinhDoanh.DiaChiHuyen ?? 0).ToString()),
                    new Claim("DiaChiTinh",  (giayDangKyKinhDoanh.DiaChiTinh ?? 0).ToString()),
                    new Claim("SoDienThoai", giayDangKyKinhDoanh.SoDienThoai ?? ""),
                    new Claim("NguoiDaiDienTen",giayDangKyKinhDoanh.NguoiDaiDienTen ?? ""),
                    new Claim("NguoiDaiDienDob",giayDangKyKinhDoanh.NguoiDaiDienDob ?? ""),
                    new Claim("NguoiDaiDienCccd",giayDangKyKinhDoanh.NguoiDaiDienCccd ?? ""),
                    new Claim("NguoiDaiDienSdt", giayDangKyKinhDoanh.NguoiDaiDienSdt ?? ""),
                    new Claim("Id", (giayDangKyKinhDoanh.Id).ToString()),
                    new Claim("Version",giayDangKyKinhDoanh.Version.ToString()),
                    new Claim("ThoiGianCapNhat",giayDangKyKinhDoanh.ThoiGianCapNhat.Value.ToString("dd/MM/yyyy")),
                    new Claim("MaPhienBan",giayDangKyKinhDoanh.MaPhienBan.ToString()),
                };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().GetByExpression(x => x.Id == giayDangKyKinhDoanh.Id).FirstOrDefaultAsync();
                update.MD5 = encryptToken;
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                var duplicate = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().GetByExpression(x => x.MaPhienBan == giayDangKyKinhDoanh.MaPhienBan && x.Id != giayDangKyKinhDoanh.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                authClaims.Add(new Claim("MD5", encryptToken));
                var tokenOnly = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api1;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã thêm mới số liệu qũy bình ổn giá vào { usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api1", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu qũy bình ổn giá vào { usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu qũy bình ổn giá vào { usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api1"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI2(CreateHeThongPhanPhoiDto createHeThongPhanPhoiDto)
        {
            try
            {
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI3(CreateSanXuatDto createSanXuatDto)
        {
            try
            {
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createSanXuatDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("ChungLoai", (createSanXuatDto.ChungLoai ?? 0).ToString()),
                        new Claim("LuongSanXuat", (createSanXuatDto.LuongSanXuat ?? 0).ToString()),
                        new Claim("Id", createSanXuatDto.Id.ToString()),
                        new Claim("MaPhienBan", createSanXuatDto.MaPhienBan.ToString()),
                        new Claim("Version", createSanXuatDto.Version .ToString()),
                        new Claim("NgayBc",createSanXuatDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<SanXuat>().GetByExpression(x => x.Id == createSanXuatDto.Id).FirstOrDefaultAsync();
                update.MD5 = encryptToken;
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                var duplicate = await _unitOfWork.GetRepository<SanXuat>().GetByExpression(x => x.MaPhienBan == createSanXuatDto.MaPhienBan && x.Id != createSanXuatDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api3;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api3", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình sản xuất xăng dầu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình sản xuất xăng dầu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api3"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI4(CreateTieuThuDto createTieuThuDto)
        {
            try
            {
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createTieuThuDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("ChungLoai", (createTieuThuDto.ChungLoai ?? 0).ToString()),
                        new Claim("LuongBan", (createTieuThuDto.LuongBan ?? 0).ToString()),
                        new Claim("LoaiBan", (createTieuThuDto.LoaiBan ?? 0).ToString()),
                        new Claim("Id", createTieuThuDto.Id.ToString()),
                        new Claim("MaPhienBan", createTieuThuDto.MaPhienBan .ToString()),
                        new Claim("Version", createTieuThuDto.Version .ToString()),
                        new Claim("NgayBc",createTieuThuDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<TieuThu>().GetByExpression(x => x.Id == createTieuThuDto.Id).FirstOrDefaultAsync();
                update.MD5 = encryptToken;
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                var duplicate = await _unitOfWork.GetRepository<TieuThu>().GetByExpression(x => x.MaPhienBan == createTieuThuDto.MaPhienBan && x.Id != createTieuThuDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api4;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api4", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình tiêu thụ vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình tiêu thụ vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api4"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI5(CreateNhapDto createNhapDto)
        {
            try
            {
                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createNhapDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("ChungLoai", (createNhapDto.ChungLoai ?? 0).ToString()),
                        new Claim("LuongNhap", (createNhapDto.LuongNhap ?? 0).ToString()),
                        new Claim("LoaiNhap", (createNhapDto.LoaiNhap ?? 0).ToString()),
                        new Claim("Id", createNhapDto.Id.ToString()),
                        new Claim("MaPhienBan", createNhapDto.MaPhienBan .ToString()),
                        new Claim("Version", createNhapDto.Version .ToString()),
                        new Claim("NgayBc",createNhapDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<Nhap>().GetByExpression(x => x.Id == createNhapDto.Id).FirstOrDefaultAsync();
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                update.AprrovedBy = curentUser.UserId;
                update.MD5 = encryptToken;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                var duplicate = await _unitOfWork.GetRepository<Nhap>().GetByExpression(x => x.MaPhienBan == createNhapDto.MaPhienBan && x.Id != createNhapDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api5;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api5", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình nhập xăng dầu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình nhập xăng dầu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api5"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI6(CreateTonKhoDto createTonKhoDto)
        {
            try
            {
                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createTonKhoDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("ChungLoai", (createTonKhoDto.ChungLoai ?? 0).ToString()),
                        new Claim("LuongTon", (createTonKhoDto.LuongTon ?? 0).ToString()),
                        new Claim("VungMien", (createTonKhoDto.VungMien ?? 0).ToString()),
                        new Claim("Id", createTonKhoDto.Id.ToString()),
                        new Claim("MaPhienBan", createTonKhoDto.MaPhienBan .ToString()),
                        new Claim("Version", createTonKhoDto.Version .ToString()),
                        new Claim("NgayBc",createTonKhoDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<TonKho>().GetByExpression(x => x.Id == createTonKhoDto.Id).FirstOrDefaultAsync();
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                update.MD5 = encryptToken;
                var duplicate = await _unitOfWork.GetRepository<TonKho>().GetByExpression(x => x.MaPhienBan == createTonKhoDto.MaPhienBan && x.Id != createTonKhoDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api6;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api6", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Tình hình tồn kho vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhân số liệu Tình hình tồn kho vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api6"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI7(CreateNguyenLieuDto createNguyenLieuDto)
        {
            try
            {

                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createNguyenLieuDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("TenNguyenLieu", createNguyenLieuDto.TenNguyenLieu ?? ""),
                        new Claim("LuongNhap", (createNguyenLieuDto.LuongNhap ?? 0).ToString()),
                        new Claim("DonViTinh", createNguyenLieuDto.DonViTinh ?? ""),
                        new Claim("Id", createNguyenLieuDto.Id.ToString()),
                        new Claim("MaPhienBan", createNguyenLieuDto.MaPhienBan.ToString()),
                        new Claim("Version", createNguyenLieuDto.Version.ToString()),
                        new Claim("NgayBc",createNguyenLieuDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<NguyenLieu>().GetByExpression(x => x.Id == createNguyenLieuDto.Id).FirstOrDefaultAsync();
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                update.MD5 = encryptToken;
                var duplicate = await _unitOfWork.GetRepository<NguyenLieu>().GetByExpression(x => x.MaPhienBan == createNguyenLieuDto.MaPhienBan && x.Id != createNguyenLieuDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api7;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api7", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Lượng nguyên liệu nhập khẩu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Lượng nguyên liệu nhập khẩu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api7"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI8(CreateKhacDto createKhacDto)
        {
            try
            {

                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createKhacDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("ChungLoai", (createKhacDto.ChungLoai ?? 0).ToString()),
                        new Claim("LuongPhaChe", (createKhacDto.LuongPhaChe ?? 0).ToString()),
                        new Claim("LuongTamNhapTaiXuat",(createKhacDto.LuongTamNhapTaiXuat ?? 0).ToString()),
                        new Claim("LuongChuyenKhau", (createKhacDto.LuongChuyenKhau ?? 0).ToString()),
                        new Claim("Id", createKhacDto.Id.ToString()),
                        new Claim("MaPhienBan", createKhacDto.MaPhienBan.ToString()),
                        new Claim("Version", createKhacDto.Version.ToString()),
                        new Claim("NgayBc",createKhacDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<Khac>().GetByExpression(x => x.Id == createKhacDto.Id).FirstOrDefaultAsync();
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                update.MD5 = encryptToken;
                var duplicate = await _unitOfWork.GetRepository<Khac>().GetByExpression(x => x.MaPhienBan == createKhacDto.MaPhienBan && x.Id != createKhacDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api8;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api8", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Báo cáo khác cho TMĐMKD vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Báo cáo khác cho TMĐMKD vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api8"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<bool>> ApproveAPI9(CreateQuyBinhOnGiumDto createQuyBinhOnGiumDto)
        {
            try
            {

                var authClaims = new List<Claim>
                    {
                        new Claim("MaDoiTuong", (createQuyBinhOnGiumDto.MaDoiTuong ?? 0).ToString()),
                        new Claim("SoDu", (createQuyBinhOnGiumDto.SoDu ?? 0).ToString()),
                        new Claim("Id", createQuyBinhOnGiumDto.Id.ToString()),
                        new Claim("MaPhienBan", createQuyBinhOnGiumDto.MaPhienBan.ToString()),
                        new Claim("Version", createQuyBinhOnGiumDto.Version.ToString()),
                        new Claim("NgayBc",createQuyBinhOnGiumDto.NgayBc.Value.ToString("dd/MM/yyyy")),
                    };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                string encryptToken = Md5Encrypt.MD5Hash(new JwtSecurityTokenHandler().WriteToken(token));
                var update = await _unitOfWork.GetRepository<QuyBinhOnGium>().GetByExpression(x => x.Id == createQuyBinhOnGiumDto.Id).FirstOrDefaultAsync();
                var curentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == curentUser.UserId);
                update.AprrovedBy = curentUser.UserId;
                update.AprrovedDate = DateTime.Now;
                update.AprrovedStatus = ApprovedStatus.Approve;
                update.MD5 = encryptToken;
                var duplicate = await _unitOfWork.GetRepository<QuyBinhOnGium>().GetByExpression(x => x.MaPhienBan == createQuyBinhOnGiumDto.MaPhienBan && x.Id != createQuyBinhOnGiumDto.Id).ToListAsync();
                foreach (var item in duplicate)
                {
                    item.IsDelete = true;
                }
                var tokenOnly = new JwtSecurityToken(
                   issuer: _configuration["JWT:ValidIssuer"],
                   audience: _configuration["JWT:ValidAudience"],
                   claims: authClaims,
                   signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                   );
                var usersReference = new UsersReference();
                usersReference.EncryptUser = new JwtSecurityTokenHandler().WriteToken(tokenOnly);
                usersReference.CreatedDate = DateTime.Now;
                usersReference.MaDoiTuong = user.MaDoiTuong;
                usersReference.ApiType = (int)ApiNumberEnum.Api9;
                await _unitOfWork.GetRepository<UsersReference>().Add(usersReference);
                await _unitOfWork.SaveAsync();
                await _actionLogsRepository.CreateAsync(new Common.Dtos.ActionLogs.CreateActionLogDto() { ContentLog = $"{curentUser.FullName} - Id: {curentUser.UserId} đã cập nhật số liệu quỹ bình ổn giá vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}", TimeLine = usersReference.CreatedDate.Value, Url = "/api9", UserId = curentUser.UserId });
                var usersCanApproved = await _roleRepository.GetGetUserCanApproveApi();
                await _notificationRepository.SendNotificationToListUser(new Notification
                {
                    Content = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Quỹ bình ổn giá xăng dầu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    CreatedBy = curentUser.UserId,
                    CreatedDate = usersReference.CreatedDate,
                    IsReaded = false,
                    Title = $"{curentUser.FullName} - Id: {curentUser.UserId} đã xác nhận số liệu Quỹ bình ổn giá xăng dầu vào {usersReference.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm")}",
                    Url = "/api9"
                }, usersCanApproved);
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }


        public async Task<Response<bool>> RejectAsync(RejectApiDto request)
        {
            try
            {
                var curentUser = await _userRepository.GetIdentityUser();
                switch (request.ApiNumber)
                {
                    case (int)ApiNumberEnum.Api1:
                        var api1 = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api1.AprrovedBy = curentUser?.UserId;
                        api1.AprrovedStatus = ApprovedStatus.Reject;
                        api1.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api2:
                        var api2 = await _unitOfWork.GetRepository<HeThongPhanPhoi>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api3:
                        var api3 = await _unitOfWork.GetRepository<SanXuat>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api3.AprrovedBy = curentUser?.UserId;
                        api3.AprrovedStatus = ApprovedStatus.Reject;
                        api3.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api4:
                        var api4 = await _unitOfWork.GetRepository<TieuThu>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api4.AprrovedBy = curentUser?.UserId;
                        api4.AprrovedStatus = ApprovedStatus.Reject;
                        api4.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api5:
                        var api5 = await _unitOfWork.GetRepository<Nhap>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api5.AprrovedBy = curentUser?.UserId;
                        api5.AprrovedStatus = ApprovedStatus.Reject;
                        api5.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;
                    case (int)ApiNumberEnum.Api6:
                        var api6 = await _unitOfWork.GetRepository<TonKho>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api6.AprrovedBy = curentUser?.UserId;
                        api6.AprrovedStatus = ApprovedStatus.Reject;
                        api6.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api7:
                        var api7 = await _unitOfWork.GetRepository<NguyenLieu>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api7.AprrovedBy = curentUser?.UserId;
                        api7.AprrovedStatus = ApprovedStatus.Reject;
                        api7.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api8:
                        var api8 = await _unitOfWork.GetRepository<Khac>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api8.AprrovedBy = curentUser?.UserId;
                        api8.AprrovedStatus = ApprovedStatus.Reject;
                        api8.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;

                    case (int)ApiNumberEnum.Api9:
                        var api9 = await _unitOfWork.GetRepository<QuyBinhOnGium>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        api9.AprrovedBy = curentUser?.UserId;
                        api9.AprrovedStatus = ApprovedStatus.Reject;
                        api9.RejectMessage = request.Message;
                        await _unitOfWork.SaveAsync();
                        break;
                    default: break;
                }
                return Response<bool>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        #endregion

        #region ExportExcel
        public async Task<List<GiayDangKyKinhDoanh>> ExportExcelApi1Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var data = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenDoanhNghiep.ToLower().Contains(request.TextSearch.ToLower()))
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.ThoiGianCapNhat.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.ThoiGianCapNhat.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<HeThongPhanPhoi>> ExportExcelApi2Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var data = await _unitOfWork.GetRepository<HeThongPhanPhoi>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuongCha == user.MaDoiTuong)
                     .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenHeThongPhanPhoi.ToLower().Contains(request.TextSearch.ToLower()))
                     .WhereIf(request.FromDate.HasValue, n => n.ThoiGianCapNhat.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.ThoiGianCapNhat.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuongCha)
                     .ThenBy(n => n.MaDoiTuongCon)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<SanXuat>> ExportExcelApi3Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<SanXuat>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<SanXuat>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<TieuThu>> ExportExcelApi4Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<TieuThu>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<TieuThu>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<Nhap>> ExportExcelApi5Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<Nhap>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<Nhap>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<TonKho>> ExportExcelApi6Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<TonKho>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<TonKho>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<NguyenLieu>> ExportExcelApi7Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<NguyenLieu>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<NguyenLieu>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<Khac>> ExportExcelApi8Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<Khac>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<Khac>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<QuyBinhOnGium>> ExportExcelApi9Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<QuyBinhOnGium>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<QuyBinhOnGium>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.ApprovedStatus.HasValue, n => n.AprrovedStatus == request.ApprovedStatus)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBc.Value.Date >= request.FromDate)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBc.Value.Date <= request.ToDate)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        public async Task<List<DuKienNhap>> ExportExcelApi10Async(GetListApiRequestDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var query = from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                            join sx in _unitOfWork.GetAsQueryable<DuKienNhap>() on dt.MaDoiTuong equals sx.MaDoiTuong
                            where (string.IsNullOrEmpty(request.TextSearch) || dt.TenDoiTuong.ToLower().Contains(request.TextSearch.ToLower()))
                            select sx;
                var data = await _unitOfWork.GetRepository<DuKienNhap>()
                     .GetAll()
                     .WhereIf(!currentUser.IsBCT && po.UserPositionId != currentUser.UserPositionId, n => n.MaDoiTuong == user.MaDoiTuong)
                     .WhereIf(request.FromDate.HasValue, n => n.NgayBC.Date >= request.FromDate.Value.Date)
                     .WhereIf(request.ToDate.HasValue, n => n.NgayBC.Date <= request.ToDate.Value.Date)
                     .OrderBy(n => n.MaDoiTuong)
                     .ToListAsync();
                return data;
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return null;
            }
        }
        #endregion

        #region So sánh giá trị

        #endregion

        #region Xóa api
        public async Task<Response<bool>> DeleteAsync(RejectApiDto request)
        {
            try
            {
                var curentUser = await _userRepository.GetIdentityUser();
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                if (po.UserPositionId != curentUser.UserPositionId)
                {
                    await _logRepository.ErrorAsync("Chỉ Admin mới có quyền xóa!");
                    return Response<bool>.CreateErrorResponse("Chỉ Admin mới có quyền xóa!");
                }
                switch (request.ApiNumber)
                {
                    case (int)ApiNumberEnum.Api1:
                        var api1 = await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<GiayDangKyKinhDoanh>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api2:
                        var api2 = await _unitOfWork.GetRepository<HeThongPhanPhoi>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<HeThongPhanPhoi>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api3:
                        var api3 = await _unitOfWork.GetRepository<SanXuat>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<SanXuat>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api4:
                        var api4 = await _unitOfWork.GetRepository<TieuThu>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<TieuThu>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api5:
                        var api5 = await _unitOfWork.GetRepository<Nhap>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<Nhap>().DeleteByExpression(x => x.Id == request.Id); ;
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);
                    case (int)ApiNumberEnum.Api6:
                        var api6 = await _unitOfWork.GetRepository<TonKho>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<TonKho>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api7:
                        var api7 = await _unitOfWork.GetRepository<NguyenLieu>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<NguyenLieu>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api8:
                        var api8 = await _unitOfWork.GetRepository<Khac>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<Khac>().DeleteByExpression(x => x.Id == request.Id);
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);

                    case (int)ApiNumberEnum.Api9:
                        var api9 = await _unitOfWork.GetRepository<QuyBinhOnGium>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<QuyBinhOnGium>().DeleteByExpression(x => x.Id == request.Id); ;
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);
                    case (int)ApiNumberEnum.Api10:
                        var api10 = await _unitOfWork.GetRepository<DuKienNhap>().GetAll().FirstOrDefaultAsync(n => n.Id == request.Id);
                        await _unitOfWork.GetRepository<DuKienNhap>().DeleteByExpression(x => x.Id == request.Id); ;
                        await _unitOfWork.SaveAsync();
                        return Response<bool>.CreateSuccessResponse(true);
                    default: break;
                }
                return Response<bool>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
        #endregion

        public async Task<Response<bool>> ImportApi2Async()
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                IFormFileCollection files = _httpContextAccessor.HttpContext.Request.Form.Files;
                if (files != null && files.Count > 0)
                {
                    IFormFile file = files[0];
                    List<HeThongPhanPhoi> heThongPhanPhois = new List<HeThongPhanPhoi>();
                    using (var ms = new MemoryStream())
                    {
                        file.CopyTo(ms);

                        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                        using (ExcelPackage excelPackage = new ExcelPackage(ms))
                        {
                            //loop all worksheets
                            ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                            for (int i = 3; i <= worksheet.Dimension.End.Row; i++)
                            {
                                var hethong = new HeThongPhanPhoi();
                                hethong.MaDoiTuongCha = user.MaDoiTuong.Value;
                                hethong.TenHeThongPhanPhoi = worksheet.Cells[i, 1].Value.ToString();
                                hethong.DiaChi = worksheet.Cells[i, 2].Value.ToString();
                                hethong.DienThoai = worksheet.Cells[i, 3].Value != null ? worksheet.Cells[i, 3].Value.ToString() : "";
                                hethong.LoaiDoiTuongCon = Convert.ToInt32(worksheet.Cells[i, 4].Value);
                                hethong.LoaiSoHuu = Convert.ToInt32(worksheet.Cells[i, 5].Value);
                                hethong.ThoiGianCapNhat = DateTime.Now;
                                hethong.IsActive = true;
                                heThongPhanPhois.Add(hethong);
                            }
                            IEnumerable<string> tenHeThongPhanPhois = heThongPhanPhois.Select(n => n.TenHeThongPhanPhoi.ToLower());
                            var existsDoiTuong = await _unitOfWork.GetAsQueryable<HeThongPhanPhoi>().Where(n => n.MaDoiTuongCha == user.MaDoiTuong.Value && tenHeThongPhanPhois.Contains(n.TenHeThongPhanPhoi.ToLower())).Select(n => n.TenHeThongPhanPhoi.ToLower()).ToListAsync();

                            // insert vào đối tượng quản lý trước
                            IEnumerable<DoiTuongQuanLy> importDt = heThongPhanPhois.Where(n => !existsDoiTuong.Contains(n.TenHeThongPhanPhoi.ToLower())).Select(n => new DoiTuongQuanLy
                            {
                                LoaiDoiTuong = n.LoaiDoiTuongCon,
                                ParentId = n.MaDoiTuongCha,
                                TenDoiTuong = n.TenHeThongPhanPhoi,
                            });

                            // lấy ra những thằng đã có
                            foreach (var item in importDt)
                            {
                                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().Add(item);
                                await _unitOfWork.SaveAsync();

                                var htpp = heThongPhanPhois.FirstOrDefault(n => n.TenHeThongPhanPhoi.ToLower() == item.TenDoiTuong.ToLower());
                                htpp.MaDoiTuongCon = doiTuong.MaDoiTuong;
                                await _unitOfWork.GetRepository<HeThongPhanPhoi>().Add(new HeThongPhanPhoi
                                {
                                    TenHeThongPhanPhoi = htpp.TenHeThongPhanPhoi,
                                    DiaChi = htpp.DiaChi,
                                    DienThoai = htpp.DienThoai,
                                    IsActive = htpp.IsActive,
                                    LoaiDoiTuongCon = htpp.LoaiDoiTuongCon,
                                    LoaiSoHuu = htpp.LoaiSoHuu,
                                    MaDoiTuongCha = htpp.MaDoiTuongCha,
                                    MaDoiTuongCon = htpp.MaDoiTuongCon,
                                    ThoiGianCapNhat = htpp.ThoiGianCapNhat,
                                });
                            }
                            await _unitOfWork.SaveAsync();
                        }
                    }
                }
                return Response<bool>.CreateSuccessResponse(true);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<bool>.CreateErrorResponse(ex);
            }
        }
    }
}
