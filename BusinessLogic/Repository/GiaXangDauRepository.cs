using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Const;
using Common.Dtos;
using Common.Dtos.GiaXangDauDoanhNgiepDtos;
using Common.Dtos.GiaXangDauDtos;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class GiaXangDauRepository : IGiaXangDauRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public GiaXangDauRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper , IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _userRepository = userRepository;

        }
        public async Task<Response<CreateGiaXangDauDto>> CreateAsync(CreateGiaXangDauDto request)
        {
            try
            {
                request.NgayCapNhat = DateTime.Now;
                request.TrangThai = TrangThai.Use;
                var mapped = _mapper.Map<CreateGiaXangDauDto, GiaXangDau>(request);
                await _unitOfWork.GetRepository<GiaXangDau>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateGiaXangDauDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateGiaXangDauDto>> EditAsync(CreateGiaXangDauDto request)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GiaXangDau>()
                    .GetByExpression(n => n.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateGiaXangDauDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                request.NgayCapNhat = DateTime.Now;
                var mapped = _mapper.Map<CreateGiaXangDauDto, GiaXangDau>(request, query);
                await _unitOfWork.GetRepository<GiaXangDau>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateGiaXangDauDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateGiaXangDauDto>> DeleteAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GiaXangDau>().GetByExpression(n => n.Id == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateGiaXangDauDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                await _unitOfWork.GetRepository<GiaXangDau>().DeleteByExpression(n => n.Id == id);
                await _unitOfWork.SaveAsync();
                return Response<CreateGiaXangDauDto>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<GiaXangDauViewDto>>>> GetGiaXangDauAsync(GiaXangDauGetPageDto request)
        {
            try
            {
                var result = (from gxd in _unitOfWork.GetRepository<GiaXangDau>().GetAll()
                              join cl in _unitOfWork.GetRepository<ChungLoai>().GetAll() on gxd.MaChungLoai equals cl.MaChungLoai
                              select new GiaXangDauViewDto
                              {
                                  MaChungLoai = cl.MaChungLoai,
                                  TenChungLoai = cl.TenChungLoai,
                                  Gia = gxd.Gia,
                                  NgayCapNhat = gxd.NgayCapNhat,
                                  VungMien = gxd.VungMien,
                                  DotBienDong = gxd.DotBienDong,
                                  TrangThai = gxd.TrangThai,
                                  DonViTinh = gxd.DonViTinh,
                                  Id = gxd.Id,
                              })
                              .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenChungLoai.Contains(request.TextSearch))
                              .WhereIf(request.Vung.HasValue && request.Vung != 0, n => n.VungMien == request.Vung);
                GetListResponseModel<List<GiaXangDauViewDto>> responseData = new GetListResponseModel<List<GiaXangDauViewDto>>(await result.CountAsync(), request.PageSize);
                var data = await result
                    .OrderBy(n => n.MaChungLoai)
                    .ThenBy(n => n.VungMien)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();
                responseData.Data = data;
                return Response<GetListResponseModel<List<GiaXangDauViewDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<GetListResponseModel<List<GiaXangDauViewDto>>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<CreateGiaXangDauDto>> GetByIdAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GiaXangDau>().GetByExpression(n => n.Id == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateGiaXangDauDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                var mapped = _mapper.Map<GiaXangDau, CreateGiaXangDauDto>(query);
                return Response<CreateGiaXangDauDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> CreateGiaXangDoanhNghiepAsync(CreateGiaXangDauDoanhNghiepDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                if (currentUser.IsBCT || po.UserPositionId == currentUser.UserPositionId)
                {
                    request.MaDoiTuong = 0;
                }
                else
                {
                    request.MaDoiTuong = user.MaDoiTuong;
                }
                request.CreatedBy = user.UserId;
                request.CreatedDate = DateTime.Now;
                var mapped = _mapper.Map<CreateGiaXangDauDoanhNghiepDto, GiaBanXangDauDoanhNghiep>(request);
                await _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> EditGiaXangDoanhNghiepAsync(CreateGiaXangDauDoanhNghiepDto request)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>()
                    .GetByExpression(n => n.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                request.ModifiedBy = user.UserId;
                request.ModifiedDate = DateTime.Now;
                var mapped = _mapper.Map<CreateGiaXangDauDoanhNghiepDto, GiaBanXangDauDoanhNghiep>(request, query);
                await _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> DeleteGiaBanDoanhNghiepAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>().GetByExpression(n => n.Id == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                await _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>().DeleteByExpression(n => n.Id == id);
                await _unitOfWork.SaveAsync();
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<GiaXangDauDoanhNghiepViewsDto>>> GetListGiaBanDoanhNghiepAsync()
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                var po = await _unitOfWork.GetRepository<UserPosition>().GetAll().FirstOrDefaultAsync(n => n.IsAdministrator.Value);
                var data = (from gxd in _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>().GetAll()
                            join cl in _unitOfWork.GetRepository<ChungLoai>().GetAll() on gxd.MaChungLoai equals cl.MaChungLoai
                            select new GiaXangDauDoanhNghiepViewsDto
                            {
                                Id = gxd.Id,
                                TenChungLoai = cl.TenChungLoai,
                                MaChungLoai = cl.MaChungLoai,
                                Gia = gxd.Gia,
                                DonViTinh = gxd.DonViTinh,
                                MaDoiTuong = gxd.MaDoiTuong
                            });
                if(currentUser.IsBCT || po.UserPositionId == currentUser.UserPositionId)
                {
                    data = data.Where(n => n.MaDoiTuong == 0);
                }
                else
                {
                    data = data.Where(n => n.MaDoiTuong == user.MaDoiTuong);
                }
                return Response<List<GiaXangDauDoanhNghiepViewsDto>>.CreateSuccessResponse(await data.ToListAsync());
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<List<GiaXangDauDoanhNghiepViewsDto>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<GiaXangDauDoanhNghiepViewsDto>>> GetPriceBctAsync()
        {
            try
            {
                var data = await (from gxd in _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>().GetAll()
                            join cl in _unitOfWork.GetRepository<ChungLoai>().GetAll() on gxd.MaChungLoai equals cl.MaChungLoai
                            where gxd.MaDoiTuong == 0
                            select new GiaXangDauDoanhNghiepViewsDto
                            {
                                Id = gxd.Id,
                                TenChungLoai = cl.TenChungLoai,
                                MaChungLoai = cl.MaChungLoai,
                                Gia = gxd.Gia,
                                DonViTinh = gxd.DonViTinh,
                                MaDoiTuong = gxd.MaDoiTuong
                            }).ToListAsync();
                return Response<List<GiaXangDauDoanhNghiepViewsDto>>.CreateSuccessResponse(data);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<List<GiaXangDauDoanhNghiepViewsDto>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateGiaXangDauDoanhNghiepDto>> GetByIdGiaXangDoanhNghiepAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<GiaBanXangDauDoanhNghiep>()
                    .GetByExpression(n => n.Id == id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(new Exception("Không tìm thấy phòng ban !"));
                }
                var mapped = _mapper.Map<GiaBanXangDauDoanhNghiep, CreateGiaXangDauDoanhNghiepDto>(query);
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateGiaXangDauDoanhNghiepDto>.CreateErrorResponse(ex);
            }
        }
    }
}
