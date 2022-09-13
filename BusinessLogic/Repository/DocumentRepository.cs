using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.DocumentDtos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class DocumentRepository : IDocumentRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public DocumentRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Response<GetListResponseModel<List<DocumentViewDto>>>> GetListAsync(DocumentGetListRequestDto request)
        {
            try
            {
                var query =  _unitOfWork.GetAsQueryable<DataAccess.Document>()
                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.Name.Contains(request.TextSearch));

                GetListResponseModel<List<DocumentViewDto>> responseData = new GetListResponseModel<List<DocumentViewDto>>(query.Count(), request.PageSize.Value);
                var data = await query
                    .OrderByDescending(n => n.CreatedDate)
                    .Skip(request.PageSize.Value * (request.PageIndex.Value - 1)).Take(request.PageSize.Value)
                    .ToListAsync();
                responseData.Data = _mapper.Map<List<DataAccess.Document>, List<DocumentViewDto>>(data);
                return Response<GetListResponseModel<List<DocumentViewDto>>>.CreateSuccessResponse(responseData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetListResponseModel<List<DocumentViewDto>>>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DocumentViewDto>> GetByIdAsync(long id)
        {
            try
            {
                var query = await _unitOfWork.GetAsQueryable<DataAccess.Document>()
                    .FirstOrDefaultAsync(n => n.Id == id);
                return Response<DocumentViewDto>.CreateSuccessResponse(_mapper.Map<DataAccess.Document, DocumentViewDto>(query));
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DocumentViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DocumentViewDto>> CreateAsync(DocumentViewDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                request.CreatedBy = currentUser.UserId;
                request.CreatedDate = DateTime.Now;
                var mapped = _mapper.Map<DocumentViewDto, DataAccess.Document>(request);
                var doc = await _unitOfWork.GetRepository<DataAccess.Document>().Add(mapped);
                await _unitOfWork.SaveAsync();
                request.Id = doc.Id;
                return Response<DocumentViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DocumentViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DocumentViewDto>> UpdateAsync(DocumentViewDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var update = await _unitOfWork.GetAsQueryable<DataAccess.Document>().FirstOrDefaultAsync(n => n.Id == request.Id);
                request.ModifiedBy = currentUser.UserId;
                request.ModifiedDate = DateTime.Now;
                var mapped = _mapper.Map<DocumentViewDto, DataAccess.Document>(request, update);
                await _unitOfWork.GetRepository<DataAccess.Document>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<DocumentViewDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DocumentViewDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<DocumentViewDto>> DeleteAsync(long id)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var update = await _unitOfWork.GetAsQueryable<DataAccess.Document>().FirstOrDefaultAsync(n => n.Id == id);
                await _unitOfWork.GetRepository<DataAccess.Document>().Delete(update);
                await _unitOfWork.SaveAsync();
                return Response<DocumentViewDto>.CreateSuccessResponse(_mapper.Map<DataAccess.Document, DocumentViewDto>(update));
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<DocumentViewDto>.CreateErrorResponse(ex);
            }
        }
    }
}
