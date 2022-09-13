using Common.Dtos;
using Common.Dtos.TongNguonPhanGiaoDtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface ITongNguonPhanGiaoRepository
    {
        Task<Response<CreateTongNguonPhanGiaoDto>> CreateAsync(CreateTongNguonPhanGiaoDto request);
        Task<Response<CreateTongNguonPhanGiaoDto>> DeleteAsync(int id);
        Task<Response<CreateTongNguonPhanGiaoDto>> EditAsync(CreateTongNguonPhanGiaoDto request);
        Task<Response<GetListResponseModel<List<TongNguonPhanGiaoViewsDto>>>> GetListsync(TongNguongPhanGiaoPageDto request);
        Task<Response<TongNguonPhanGiaoViewsDto>> GetByIdAsync(int id);
    }
}