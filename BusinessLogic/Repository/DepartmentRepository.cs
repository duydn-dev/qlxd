using AutoMapper;
using Microsoft.EntityFrameworkCore;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.DepartmentDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        public DepartmentRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
        }

        public async Task<Response<DepartmentViewDto>> CreateAsync(DepartmentViewDto request)
        {
            try
            {
                request.DepartmentId = Guid.NewGuid();
                var mapped = _mapper.Map<DepartmentViewDto, Department>(request);
                await _unitOfWork.GetRepository<Department>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DepartmentViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DepartmentViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<DepartmentViewDto>> DeleteAsync(Guid id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<Department>().GetByExpression(n => n.DepartmentId == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<DepartmentViewDto>.CreateErrorResponse(new Exception("Không tìm thấy phòng ban !"));
                }
                var mapped = _mapper.Map<Department, DepartmentViewDto>(query);
                await _unitOfWork.GetRepository<Department>().DeleteByExpression(n => n.DepartmentId == id);
                await _unitOfWork.SaveAsync();
                return Response<DepartmentViewDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DepartmentViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<DepartmentViewDto>> EditAsync(DepartmentViewDto request)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<Department>()
                    .GetByExpression(n => n.DepartmentId == request.DepartmentId)
                    .FirstOrDefaultAsync();
                if(query == null)
                {
                    return Response<DepartmentViewDto>.CreateErrorResponse(new Exception("Không tìm thấy phòng ban !"));
                }
                var mapped = _mapper.Map<DepartmentViewDto, Department>(request, query);
                await _unitOfWork.GetRepository<Department>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DepartmentViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DepartmentViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<DepartmentViewDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<Department>()
                    .GetByExpression(n => n.DepartmentId == id)
                    .FirstOrDefaultAsync();
                if(query == null)
                {
                    return Response<DepartmentViewDto>.CreateErrorResponse(new Exception("Không tìm thấy phòng ban !"));
                }
                var mapped = _mapper.Map<Department, DepartmentViewDto>(query);
                return Response<DepartmentViewDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<DepartmentViewDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<GetListResponseModel<List<DepartmentViewDto>>>> GetDepartments(DepartmentGetPageDto request)
        {
            try
            {
                var query = _unitOfWork.GetRepository<Department>()
                    .GetAll()
                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.DepartmentName.Contains(request.TextSearch));

                GetListResponseModel<List<DepartmentViewDto>> responseData = new GetListResponseModel<List<DepartmentViewDto>> (query.Count(), request.PageSize);
                var result = await query
                    .OrderByDescending(n => n.CreatedDate)
                    .Skip(request.PageSize * (request.PageIndex - 1)).Take(request.PageSize)
                    .ToListAsync();

                responseData.Data = _mapper.Map<List<Department>, List<DepartmentViewDto>>(result);

                return Response<GetListResponseModel<List<DepartmentViewDto>>>.CreateSuccessResponse(responseData);
            }
            catch(Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<GetListResponseModel<List<DepartmentViewDto>>>.CreateErrorResponse(ex);
            }
        }
    }
}
