using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class DoiTuongQuanLyRepository : IDoiTuongQuanLyRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public DoiTuongQuanLyRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _userRepository = userRepository;   
        }
        public async Task<Response<DoiTuongQuanLyViewDto>> CreateAsync(DoiTuongQuanLyViewDto request)
        {
            try
            {
                var mapped = _mapper.Map<DoiTuongQuanLyViewDto, DoiTuongQuanLy>(request);
                await _unitOfWork.GetRepository<DoiTuongQuanLy>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DoiTuongQuanLyViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DoiTuongQuanLyViewDto>> EditAsync(DoiTuongQuanLyViewDto request)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<DoiTuongQuanLy>()
                    .GetByExpression(n => n.MaDoiTuong == request.MaDoiTuong)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                var mapped = _mapper.Map<DoiTuongQuanLyViewDto, DoiTuongQuanLy>(request, query);
                await _unitOfWork.GetRepository<DoiTuongQuanLy>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DoiTuongQuanLyViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DoiTuongQuanLyViewDto>> DeleteAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetByExpression(n => n.MaDoiTuong == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                await _unitOfWork.GetRepository<DoiTuongQuanLy>().DeleteByExpression(n => n.MaDoiTuong == id);
                await _unitOfWork.SaveAsync();
                return Response<DoiTuongQuanLyViewDto>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<GetListResponseModel<List<DoiTuongQuanLyViewDto>>>> GetQuanLyDoiTuong(DoiTuongQuanLyGetPageDto request)
        {
            try
            {
                var query = _unitOfWork.GetRepository<DoiTuongQuanLy>()
                    .GetAll()
                    .WhereIf(request.LoaiDoiTuong.HasValue, n => n.LoaiDoiTuong == request.LoaiDoiTuong)
                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.TenDoiTuong.Contains(request.TextSearch));

                GetListResponseModel<List<DoiTuongQuanLyViewDto>> responseData = new GetListResponseModel<List<DoiTuongQuanLyViewDto>>(query.Count(), request.PageSize);
                var result = await query
                    .OrderBy(n => n.MaDoiTuong)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = _mapper.Map<List<DoiTuongQuanLy>, List<DoiTuongQuanLyViewDto>>(result);

                return Response<GetListResponseModel<List<DoiTuongQuanLyViewDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<GetListResponseModel<List<DoiTuongQuanLyViewDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DoiTuongQuanLyViewDto>> GetByIdAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<DoiTuongQuanLy>()
                    .GetByExpression(n => n.MaDoiTuong == id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(new Exception("Không tìm thấy phòng ban !"));
                }
                var mapped = _mapper.Map<DoiTuongQuanLy, DoiTuongQuanLyViewDto>(query);
                return Response<DoiTuongQuanLyViewDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<List<DoiTuongQuanLy>> ListDoiTuongAsync()
        {
            return await _unitOfWork.GetAsQueryable<DoiTuongQuanLy>().ToListAsync();
        }

        public async Task<Response<DoiTuongQuanLyViewDto>> CreateChildAsync(DoiTuongQuanLyViewDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var doiTuong = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == currentUser.UserId).FirstOrDefaultAsync();
                request.ParentId = doiTuong.MaDoiTuong.Value;
                var mapped = _mapper.Map<DoiTuongQuanLyViewDto, DoiTuongQuanLy>(request);
                var res = await _unitOfWork.GetRepository<DoiTuongQuanLy>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DoiTuongQuanLyViewDto>.CreateSuccessResponse(_mapper.Map<DoiTuongQuanLy, DoiTuongQuanLyViewDto>(mapped));
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DoiTuongQuanLyViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<List<DoiTuongQuanLy>>> ListDoiTuongConAsync()
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var doiTuong = await _unitOfWork.GetRepository<User>().GetByExpression(n => n.UserId == currentUser.UserId).FirstOrDefaultAsync();
                return Response<List<DoiTuongQuanLy>>.CreateSuccessResponse(await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(n => n.ParentId == doiTuong.MaDoiTuong).ToListAsync());
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<List<DoiTuongQuanLy>>.CreateErrorResponse(ex);
            }
        }
    }
}
