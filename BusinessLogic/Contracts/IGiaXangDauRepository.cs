using Common.Dtos;
using Common.Dtos.GiaXangDauDoanhNgiepDtos;
using Common.Dtos.GiaXangDauDtos;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IGiaXangDauRepository
    {
        Task<Response<CreateGiaXangDauDto>> CreateAsync(CreateGiaXangDauDto request);
        Task<Response<CreateGiaXangDauDto>> EditAsync(CreateGiaXangDauDto request);
        Task<Response<CreateGiaXangDauDto>> DeleteAsync(int id);
        Task<Response<CreateGiaXangDauDto>> GetByIdAsync(int id);
        Task<Response<GetListResponseModel<List<GiaXangDauViewDto>>>> GetGiaXangDauAsync(GiaXangDauGetPageDto request);
        Task<Response<CreateGiaXangDauDoanhNghiepDto>> CreateGiaXangDoanhNghiepAsync(CreateGiaXangDauDoanhNghiepDto request);
        Task<Response<CreateGiaXangDauDoanhNghiepDto>> EditGiaXangDoanhNghiepAsync(CreateGiaXangDauDoanhNghiepDto request);
        Task<Response<CreateGiaXangDauDoanhNghiepDto>> DeleteGiaBanDoanhNghiepAsync(int id);
        Task<Response<List<GiaXangDauDoanhNghiepViewsDto>>> GetListGiaBanDoanhNghiepAsync();
        Task<Response<List<GiaXangDauDoanhNghiepViewsDto>>> GetPriceBctAsync();
        Task<Response<CreateGiaXangDauDoanhNghiepDto>> GetByIdGiaXangDoanhNghiepAsync(int id);
    }
}
