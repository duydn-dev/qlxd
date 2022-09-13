using Common.Dtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using DataAccess;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IDoiTuongQuanLyRepository
    {
        Task<Response<DoiTuongQuanLyViewDto>> CreateAsync(DoiTuongQuanLyViewDto request);
        Task<Response<DoiTuongQuanLyViewDto>> DeleteAsync(int id);
        Task<Response<DoiTuongQuanLyViewDto>> EditAsync(DoiTuongQuanLyViewDto request);
        Task<Response<DoiTuongQuanLyViewDto>> GetByIdAsync(int id);
        Task<Response<GetListResponseModel<List<DoiTuongQuanLyViewDto>>>> GetQuanLyDoiTuong(DoiTuongQuanLyGetPageDto request);
        Task<List<DoiTuongQuanLy>> ListDoiTuongAsync();
        Task<Response<DoiTuongQuanLyViewDto>> CreateChildAsync(DoiTuongQuanLyViewDto request);
        Task<Response<List<DoiTuongQuanLy>>> ListDoiTuongConAsync();
    }
}