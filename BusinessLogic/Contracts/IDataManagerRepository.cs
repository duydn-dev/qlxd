using Common.Dtos;
using Common.Dtos.CreateApiDtos;
using Common.Dtos.DataManagerDtos;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Contracts
{
    public interface IDataManagerRepository
    {
        Task<Response<GetListResponseModel<List<ListApiDataResponseDto>>>> GetFilterApi1Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<HeThongPhanPhoiDto>>>> GetFilterApi2Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<SanXuatDto>>>> GetFilterApi3Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<TieuThuDto>>>> GetFilterApi4Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<NhapDto>>>> GetFilterApi5Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<TonKhoDto>>>> GetFilterApi6Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<NguyenLieuDto>>>> GetFilterApi7Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<KhacDto>>>> GetFilterApi8Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<QuyBinhOnGiaDto>>>> GetFilterApi9Async(GetListApiRequestDto request);
        Task<Response<GetListResponseModel<List<DuKienNhapDto>>>> GetFilterApi10Async(GetListApiRequestDto request);
        Task<Response<CreateGiayDangKyKinhdoanhDto>> CreateApi1Async(CreateGiayDangKyKinhdoanhDto request);
        Task<Response<CreateHeThongPhanPhoiDto>> CreateApi2Async(CreateHeThongPhanPhoiDto request);
        Task<Response<CreateSanXuatDto>> CreateApi3Async(CreateSanXuatDto request);
        Task<Response<CreateTieuThuDto>> CreateApi4Async(CreateTieuThuDto request);
        Task<Response<CreateNhapDto>> CreateApi5Async(CreateNhapDto request);
        Task<Response<CreateTonKhoDto>> CreateApi6Async(CreateTonKhoDto request);
        Task<Response<CreateNguyenLieuDto>> CreateApi7Async(CreateNguyenLieuDto request);
        Task<Response<CreateKhacDto>> CreateApi8Async(CreateKhacDto request);
        Task<Response<CreateQuyBinhOnGiumDto>> CreateApi9Async(CreateQuyBinhOnGiumDto request);
        Task<Response<CreateDuKienNhapDto>> CreateApi10Async(CreateDuKienNhapDto request);
        Task<Response<List<ChungLoaiDto>>> ListChungLoaiAsync();
        Task<Response<CreateGiayDangKyKinhdoanhDto>> UpdateApi1Async(CreateGiayDangKyKinhdoanhDto request);
        Task<Response<CreateHeThongPhanPhoiDto>> UpdateApi2Async(CreateHeThongPhanPhoiDto request);
        Task<Response<CreateSanXuatDto>> UpdateApi3Async(CreateSanXuatDto request);
        Task<Response<CreateTieuThuDto>> UpdateApi4Async(CreateTieuThuDto request);
        Task<Response<CreateNhapDto>> UpdateApi5Async(CreateNhapDto request);
        Task<Response<CreateTonKhoDto>> UpdateApi6Async(CreateTonKhoDto request);
        Task<Response<CreateNguyenLieuDto>> UpdateApi7Async(CreateNguyenLieuDto request);
        Task<Response<CreateKhacDto>> UpdateApi8Async(CreateKhacDto request);
        Task<Response<CreateQuyBinhOnGiumDto>> UpdateApi9Async(CreateQuyBinhOnGiumDto request);
        Task<Response<CreateDuKienNhapDto>> UpdateApi10Async(CreateDuKienNhapDto request);
        Task<Response<bool>> ApproveAPI1(CreateGiayDangKyKinhdoanhDto giayDangKyKinhDoanh);
        Task<Response<bool>> ApproveAPI2(CreateHeThongPhanPhoiDto createHeThongPhanPhoiDto);
        Task<Response<bool>> ApproveAPI3(CreateSanXuatDto createSanXuatDto);
        Task<Response<bool>> ApproveAPI4(CreateTieuThuDto createTieuThuDto);
        Task<Response<bool>> ApproveAPI5(CreateNhapDto createNhapDto);
        Task<Response<bool>> ApproveAPI6(CreateTonKhoDto createTonKhoDto);
        Task<Response<bool>> ApproveAPI7(CreateNguyenLieuDto createNguyenLieuDto);
        Task<Response<bool>> ApproveAPI8(CreateKhacDto createKhacDto);
        Task<Response<bool>> ApproveAPI9(CreateQuyBinhOnGiumDto createQuyBinhOnGiumDto);
        Task<Response<bool>> RejectAsync(RejectApiDto request);
        Task<Response<object>> GetById(GetByIdRequestDto request);
        Task<List<GiayDangKyKinhDoanh>> ExportExcelApi1Async(GetListApiRequestDto request);
        Task<List<HeThongPhanPhoi>> ExportExcelApi2Async(GetListApiRequestDto request);
        Task<List<SanXuat>> ExportExcelApi3Async(GetListApiRequestDto request);
        Task<List<TieuThu>> ExportExcelApi4Async(GetListApiRequestDto request);
        Task<List<Nhap>> ExportExcelApi5Async(GetListApiRequestDto request);
        Task<List<TonKho>> ExportExcelApi6Async(GetListApiRequestDto request);
        Task<List<NguyenLieu>> ExportExcelApi7Async(GetListApiRequestDto request);
        Task<List<Khac>> ExportExcelApi8Async(GetListApiRequestDto request);
        Task<List<QuyBinhOnGium>> ExportExcelApi9Async(GetListApiRequestDto request);
        Task<List<DuKienNhap>> ExportExcelApi10Async(GetListApiRequestDto request);
        Task<Response<bool>> DeleteAsync(RejectApiDto request);
        Task<Response<bool>> ImportApi2Async();
    }
}
