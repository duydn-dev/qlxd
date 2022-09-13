using AutoMapper;
using AutoMapper.Configuration;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.TongNguonPhanGiaoDtos;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class TongNguonPhanGiaoRepository : ITongNguonPhanGiaoRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public TongNguonPhanGiaoRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Response<CreateTongNguonPhanGiaoDto>> CreateAsync(CreateTongNguonPhanGiaoDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                request.CreatedBy = currentUser.UserId;
                request.CreatedDate = DateTime.Now;
                var mapped = _mapper.Map<CreateTongNguonPhanGiaoDto, TongNguonPhanGiao>(request);
                await _unitOfWork.GetRepository<TongNguonPhanGiao>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateTongNguonPhanGiaoDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTongNguonPhanGiaoDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateTongNguonPhanGiaoDto>> EditAsync(CreateTongNguonPhanGiaoDto request)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<TongNguonPhanGiao>()
                    .GetByExpression(n => n.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateTongNguonPhanGiaoDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                var currentUser = await _userRepository.GetIdentityUser();
                request.ModifiedBy = currentUser.UserId;
                request.ModifiedDate = DateTime.Now;
                var mapped = _mapper.Map<CreateTongNguonPhanGiaoDto, TongNguonPhanGiao>(request, query);
                await _unitOfWork.GetRepository<TongNguonPhanGiao>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateTongNguonPhanGiaoDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTongNguonPhanGiaoDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateTongNguonPhanGiaoDto>> DeleteAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<TongNguonPhanGiao>().GetByExpression(n => n.Id == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateTongNguonPhanGiaoDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                await _unitOfWork.GetRepository<TongNguonPhanGiao>().DeleteByExpression(n => n.Id == id);
                await _unitOfWork.SaveAsync();
                return Response<CreateTongNguonPhanGiaoDto>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateTongNguonPhanGiaoDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<TongNguonPhanGiaoViewsDto>>>> GetListsync(TongNguongPhanGiaoPageDto request)
        {
            try
            {
                var result = (from gxd in _unitOfWork.GetRepository<TongNguonPhanGiao>().GetAll()
                              join cl in _unitOfWork.GetRepository<ChungLoai>().GetAll() on gxd.MaChungLoai equals cl.MaChungLoai
                              join dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>() on gxd.MaDoiTuong equals dt.MaDoiTuong
                              select new TongNguonPhanGiaoViewsDto
                              {
                                  Id = gxd.Id,
                                  MaChungLoai = cl.MaChungLoai,
                                  TenChungLoai = cl.TenChungLoai,
                                  SoLuong = gxd.SoLuong,
                                  Nam = gxd.Nam,
                                  MaDoiTuong = gxd.MaDoiTuong,
                                  TenDoiTuong = dt.TenDoiTuong
                              })
                              .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenChungLoai.Contains(request.TextSearch))
                              .WhereIf(request.Nam.HasValue && request.Nam != 0, n => n.Nam == request.Nam);
                GetListResponseModel<List<TongNguonPhanGiaoViewsDto>> responseData = new GetListResponseModel<List<TongNguonPhanGiaoViewsDto>>(await result.CountAsync(), request.PageSize);
                var data = await result
                    .OrderBy(n => n.MaDoiTuong)
                    .ThenBy(n => n.Nam)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();
                responseData.Data = data;
                return Response<GetListResponseModel<List<TongNguonPhanGiaoViewsDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<GetListResponseModel<List<TongNguonPhanGiaoViewsDto>>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<TongNguonPhanGiaoViewsDto>> GetByIdAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<TongNguonPhanGiao>().GetByExpression(n => n.Id == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<TongNguonPhanGiaoViewsDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                var mapped = _mapper.Map<TongNguonPhanGiao, TongNguonPhanGiaoViewsDto>(query);
                return Response<TongNguonPhanGiaoViewsDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<TongNguonPhanGiaoViewsDto>.CreateErrorResponse(ex);
            }
        }
    }
}
