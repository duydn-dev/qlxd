using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Dtos;
using Common.Dtos.StoreHouseDtos;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Repository
{
    public class StoreHouseRepository : IStoreHouseRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        public StoreHouseRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IUserRepository userRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _userRepository = userRepository;
        }
        public async Task<Response<object>> GetListAsync(StoreHouseGetListRequestDto request)
        {
            try
            {
                var result = new List<StoreHouseGetListDto>();
                var cates = await _unitOfWork.GetAsQueryable<StoreHouseCategory>().ToListAsync();
                var storeHouse = _unitOfWork.GetAsQueryable<StoreHouse>()
                    .WhereIf(!string.IsNullOrEmpty(request.TextSearch), n => n.StoreName.Contains(request.TextSearch));
                foreach (var item in cates)
                {
                    var cate = new StoreHouseGetListDto
                    {
                        Id = item.Id,
                        StoreName = item.NumNo + ". " + item.Name,
                        Capacity = 0,
                        Dwt = "",
                        IsCate = true,
                    };
                    if(item.LocalityId != null && item.LocalityId != 0)
                    {
                        var lstStore = (await storeHouse.Where(n => n.StoreHouseCategoryId == item.Id).ToListAsync()).Select((n, i) => new StoreHouseGetListDto
                        {
                            Index= (i + 1),
                            Id = n.Id,
                            Total = n.Total,
                            Priority = n.Priority,
                            StoreName = n.StoreName,
                            Address = n.Address,
                            UnitManager = n.UnitManager,
                            Wattage = n.Wattage,
                            Capacity = n.Capacity,
                            Dwt = n.Dwt,
                            Nature = n.Nature,
                            ZoneOfInfluence = n.ZoneOfInfluence,
                            StoreHouseCategoryId = n.StoreHouseCategoryId,
                            IsCate = false
                        });
                        cate.Capacity = lstStore.Select(n => n.Capacity).Sum();
                        result.Add(cate);
                        result.AddRange(lstStore);
                    }
                    else result.Add(cate);
                }
                return Response<object>.CreateSuccessResponse(result);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<object>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateStoreHouseDto>>CreateAsync(CreateStoreHouseDto request)
        {
            try
            {
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                request.CreatedBy = user.UserId;
                request.CreatedDate = DateTime.Now;
                var mapped = _mapper.Map<CreateStoreHouseDto, StoreHouse>(request);
                await _unitOfWork.GetRepository<StoreHouse>().Add(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateStoreHouseDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateStoreHouseDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateStoreHouseDto>> EditAsync(CreateStoreHouseDto request)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<StoreHouse>()
                    .GetByExpression(n => n.Id == request.Id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateStoreHouseDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                var currentUser = await _userRepository.GetIdentityUser();
                var user = await _unitOfWork.GetRepository<User>().GetAll().FirstOrDefaultAsync(n => n.UserId == currentUser.UserId);
                request.ModifiedBy = user.UserId;
                request.ModifiedDate = DateTime.Now;
                var mapped = _mapper.Map<CreateStoreHouseDto, StoreHouse>(request, query);
                await _unitOfWork.GetRepository<StoreHouse>().Update(mapped);
                await _unitOfWork.SaveAsync();
                return Response<CreateStoreHouseDto>.CreateSuccessResponse(request);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateStoreHouseDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateStoreHouseDto>> DeleteAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<StoreHouse>().GetByExpression(n => n.Id == id).FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateStoreHouseDto>.CreateErrorResponse(new Exception("Không tìm thấy đối tượng !"));
                }
                await _unitOfWork.GetRepository<StoreHouse>().DeleteByExpression(n => n.Id == id);
                await _unitOfWork.SaveAsync();
                return Response<CreateStoreHouseDto>.CreateSuccessResponse();
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateStoreHouseDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<CreateStoreHouseDto>> GetByIdAsync(int id)
        {
            try
            {
                var query = await _unitOfWork.GetRepository<StoreHouse>()
                    .GetByExpression(n => n.Id == id)
                    .FirstOrDefaultAsync();
                if (query == null)
                {
                    return Response<CreateStoreHouseDto>.CreateErrorResponse(new Exception("Không tìm thấy phòng ban !"));
                }
                var mapped = _mapper.Map<StoreHouse, CreateStoreHouseDto>(query);
                return Response<CreateStoreHouseDto>.CreateSuccessResponse(mapped);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<CreateStoreHouseDto>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<List<StoreHouseCategory>>> GetListDropDown()
        {
            try
            {
                var result = await _unitOfWork.GetRepository<StoreHouseCategory>().GetByExpression(x => x.LocalityId != 0 && x.LocalityId != null).ToListAsync();
                return Response<List<StoreHouseCategory>>.CreateSuccessResponse(result);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<List<StoreHouseCategory>>.CreateErrorResponse(ex);
            }
        }
    }
}
