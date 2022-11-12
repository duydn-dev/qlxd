using Api.Attributes;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common.Const;
using Common.Dtos;
using Common.Dtos.CreateApiDtos;
using Common.Dtos.DataManagerDtos;
using DataAccess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [UserAuthorize]
    [Route("api/[controller]")]
    [ApiController]
    [RoleGroupDescription("Quản lý API")]
    public class DataManagerController : ControllerBase
    {
        private readonly IDataManagerRepository _dataManagerRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IUnitOfWork _unitOfWork;
        public DataManagerController(IDataManagerRepository dataManagerRepository, IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork)
        {
            _dataManagerRepository = dataManagerRepository;
            _webHostEnvironment = webHostEnvironment;
            _unitOfWork = unitOfWork;
        }
        [RoleDescription("Xem danh sách Giấy đăng kí kinh doanh")]
        [Route("api1")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<ListApiDataResponseDto>>>> GetFilterApi1([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi1Async(request);
        }
        [RoleDescription("Xem danh sách Hệ thống phân phối")]
        [Route("api2")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<HeThongPhanPhoiDto>>>> GetFilterApi2([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi2Async(request);
        }
        [RoleDescription("Xem danh sách Tình hình sản xuất xăng dầu")]
        [Route("api3")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<SanXuatDto>>>> GetFilterApi3([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi3Async(request);
        }
        [RoleDescription("Xem danh sách Tình hình tiêu thụ")]
        [Route("api4")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<TieuThuDto>>>> GetFilterApi4([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi4Async(request);
        }
        [RoleDescription("Xem danh sách Tình hình nhập xăng dầu")]
        [Route("api5")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<NhapDto>>>> GetFilterApi5([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi5Async(request);
        }
        [RoleDescription("Xem danh sách Tình hình tồn kho")]
        [Route("api6")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<TonKhoDto>>>> GetFilterApi6([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi6Async(request);
        }
        [RoleDescription("Xem danh sách Lượng nguyên liệu nhập khẩu")]
        [Route("api7")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<NguyenLieuDto>>>> GetFilterApi7([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi7Async(request);
        }
        [RoleDescription("Xem danh sách Báo cáo khác cho TMĐMKD")]
        [Route("api8")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<KhacDto>>>> GetFilterApi8([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi8Async(request);
        }
        [RoleDescription("Xem danh sách Quỹ bình ổn giá xăng dầu")]
        [Route("api9")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<QuyBinhOnGiaDto>>>> GetFilterApi9([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi9Async(request);
        }
        [RoleDescription("Xem danh sách Dự kiến nhập")]
        [Route("api10")]
        [HttpPost]
        public async Task<Response<GetListResponseModel<List<DuKienNhapDto>>>> GetFilterApi10([FromBody] GetListApiRequestDto request)
        {
            return await _dataManagerRepository.GetFilterApi10Async(request);
        }

        [RoleDescription("Xem thông tin api bằng mã")]
        [Route("get-by-id")]
        [HttpPost]
        public async Task<Response<object>> GetById([FromBody] GetByIdRequestDto request)
        {
            return await _dataManagerRepository.GetById(request);
        }

        [RoleDescription("Tạo mới Giấy đăng kí kinh doanh")]
        [HttpPost]
        [Route("create-api1")]
        public async Task<Response<CreateGiayDangKyKinhdoanhDto>> CreateApi1([FromBody] CreateGiayDangKyKinhdoanhDto request)
        {
            return await _dataManagerRepository.CreateApi1Async(request);
        }
        [RoleDescription("Tạo mới Hệ thống phân phối")]
        [HttpPost]
        [Route("create-api2")]
        public async Task<Response<CreateHeThongPhanPhoiDto>> CreateApi2([FromBody] CreateHeThongPhanPhoiDto request)
        {
            return await _dataManagerRepository.CreateApi2Async(request);
        }
        [RoleDescription("Tạo mới Tình hình sản xuất xăng dầu")]
        [HttpPost]
        [Route("create-api3")]
        public async Task<Response<CreateSanXuatDto>> CreateApi3([FromBody] CreateSanXuatDto request)
        {
            return await _dataManagerRepository.CreateApi3Async(request);
        }
        [RoleDescription("Tạo mới Tình hình tiêu thụ")]
        [HttpPost]
        [Route("create-api4")]
        public async Task<Response<CreateTieuThuDto>> CreateApi4([FromBody] CreateTieuThuDto request)
        {
            return await _dataManagerRepository.CreateApi4Async(request);
        }
        [RoleDescription("Tạo mới Tình hình nhập xăng dầu")]
        [HttpPost]
        [Route("create-api5")]
        public async Task<Response<CreateNhapDto>> CreateApi5([FromBody] CreateNhapDto request)
        {
            return await _dataManagerRepository.CreateApi5Async(request);
        }
        [RoleDescription("Tạo mới Tình hình tồn kho")]
        [HttpPost]
        [Route("create-api6")]
        public async Task<Response<CreateTonKhoDto>> CreateApi6([FromBody] CreateTonKhoDto request)
        {
            return await _dataManagerRepository.CreateApi6Async(request);
        }
        [RoleDescription("Tạo mới Lượng nguyên liệu nhập khẩu")]
        [HttpPost]
        [Route("create-api7")]
        public async Task<Response<CreateNguyenLieuDto>> CreateApi7([FromBody] CreateNguyenLieuDto request)
        {
            return await _dataManagerRepository.CreateApi7Async(request);
        }
        [RoleDescription("Tạo mới Báo cáo khác cho TMĐMKD")]
        [HttpPost]
        [Route("create-api8")]
        public async Task<Response<CreateKhacDto>> CreateApi8([FromBody] CreateKhacDto request)
        {
            return await _dataManagerRepository.CreateApi8Async(request);
        }
        [RoleDescription("Tạo mới Quỹ bình ổn giá xăng dầu")]
        [HttpPost]
        [Route("create-api9")]
        public async Task<Response<CreateQuyBinhOnGiumDto>> CreateApi9([FromBody] CreateQuyBinhOnGiumDto request)
        {
            return await _dataManagerRepository.CreateApi9Async(request);
        }
        [RoleDescription("Tạo mới Dự kiến nhập kỳ tới")]
        [HttpPost]
        [Route("create-api10")]
        public async Task<Response<CreateDuKienNhapDto>> CreateApi10([FromBody] CreateDuKienNhapDto request)
        {
            return await _dataManagerRepository.CreateApi10Async(request);
        }
        [AuthenticationOnly]
        [Route("list-chungloai")]
        [HttpGet]
        public async Task<Response<List<ChungLoaiDto>>> ListChungLoaiAsync()
        {
            return await _dataManagerRepository.ListChungLoaiAsync();
        }
        [RoleDescription("Cập nhật Giấy đăng kí kinh doanh")]
        [HttpPost]
        [Route("update-api1/{id}")]
        public async Task<Response<CreateGiayDangKyKinhdoanhDto>> UpdateApi1Async([FromRoute]int id ,[FromBody]CreateGiayDangKyKinhdoanhDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi1Async(request);
        }
        [RoleDescription("Cập nhật Hệ thống phân phối")]
        [HttpPost]
        [Route("update-api2/{id}")]
        public async Task<Response<CreateHeThongPhanPhoiDto>> UpdateApi2Async([FromRoute] int id, [FromBody] CreateHeThongPhanPhoiDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi2Async(request);
        }
        [RoleDescription("Cập nhật Tình hình sản xuất xăng dầu")]
        [HttpPost]
        [Route("update-api3/{id}")]
        public async Task<Response<CreateSanXuatDto>> UpdateApi3Async([FromRoute] int id, [FromBody] CreateSanXuatDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi3Async(request);
        }
        [RoleDescription("Cập nhật Tình hình tiêu thụ")]
        [HttpPost]
        [Route("update-api4/{id}")]
        public async Task<Response<CreateTieuThuDto>> UpdateApi4Async([FromRoute] int id, [FromBody] CreateTieuThuDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi4Async(request);
        }
        [RoleDescription("Cập nhật Tình hình nhập xăng dầu")]
        [HttpPost]
        [Route("update-api5/{id}")]
        public async Task<Response<CreateNhapDto>> UpdateApi5Async([FromRoute] int id, [FromBody] CreateNhapDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi5Async(request);
        }
        [RoleDescription("Cập nhật Tình hình tồn kho")]
        [HttpPost]
        [Route("update-api6/{id}")]
        public async Task<Response<CreateTonKhoDto>> UpdateApi6Async([FromRoute] int id, [FromBody] CreateTonKhoDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi6Async(request);
        }
        [RoleDescription("Cập nhật Lượng nguyên liệu nhập khẩu")]
        [HttpPost]
        [Route("update-api7/{id}")]
        public async Task<Response<CreateNguyenLieuDto>> UpdateApi7Async([FromRoute] int id, [FromBody] CreateNguyenLieuDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi7Async(request);
        }
        [RoleDescription("Cập nhật Báo cáo khác cho TMĐMKD")]
        [HttpPost]
        [Route("update-api8/{id}")]
        public async Task<Response<CreateKhacDto>> UpdateApi8Async([FromRoute] int id, [FromBody] CreateKhacDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi8Async(request);
        }
        [RoleDescription("Cập nhật Quỹ bình ổn giá xăng dầu")]
        [HttpPost]
        [Route("update-api9/{id}")]
        public async Task<Response<CreateQuyBinhOnGiumDto>> UpdateApi9Async([FromRoute] int id, [FromBody] CreateQuyBinhOnGiumDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi9Async(request);
        }
        [RoleDescription("Cập nhật Dự kiến nhập kỳ tới")]
        [HttpPost]
        [Route("update-api10/{id}")]
        public async Task<Response<CreateDuKienNhapDto>> UpdateApi10Async([FromRoute] int id, [FromBody] CreateDuKienNhapDto request)
        {
            request.Id = id;
            return await _dataManagerRepository.UpdateApi10Async(request);
        }
        [RoleDescription("Xác nhận Giấy đăng kí kinh doanh")]
        [HttpPost]
        [Route("xac-nhan-api1")]
        public async Task<Response<bool>> ApproveAPI1(CreateGiayDangKyKinhdoanhDto giayDangKyKinhDoanh)
        {
            return await _dataManagerRepository.ApproveAPI1(giayDangKyKinhDoanh);
        }
        [RoleDescription("Xác nhận Hệ thống phân phối")]
        [HttpPost]
        [Route("xac-nhan-api2")]
        public async Task<Response<bool>> ApproveAPI2(CreateHeThongPhanPhoiDto createHeThongPhanPhoiDto)
        {
            return await _dataManagerRepository.ApproveAPI2(createHeThongPhanPhoiDto);
        }
        [RoleDescription("Xác nhận Tình hình sản xuất xăng dầu")]
        [HttpPost]
        [Route("xac-nhan-api3")]
        public async Task<Response<bool>> ApproveAPI3(CreateSanXuatDto createSanXuatDto)
        {
            return await _dataManagerRepository.ApproveAPI3(createSanXuatDto);
        }
        [RoleDescription("Xác nhận Tình hình tiêu thụ")]
        [HttpPost]
        [Route("xac-nhan-api4")]
        public async Task<Response<bool>> ApproveAPI4(CreateTieuThuDto createTieuThuDto)
        {
            return await _dataManagerRepository.ApproveAPI4(createTieuThuDto);
        }
        [RoleDescription("Xác nhận Tình hình nhập xăng dầu")]
        [HttpPost]
        [Route("xac-nhan-api5")]
        public async Task<Response<bool>> ApproveAPI5(CreateNhapDto createNhapDto)
        {
            return await _dataManagerRepository.ApproveAPI5(createNhapDto);
        }
        [RoleDescription("Xác nhận Tình hình tồn kho")]
        [HttpPost]
        [Route("xac-nhan-api6")]
        public async Task<Response<bool>> ApproveAPI6(CreateTonKhoDto createTonKhoDto)
        {
            return await _dataManagerRepository.ApproveAPI6(createTonKhoDto);
        }
        [RoleDescription("Xác nhận Lượng nguyên liệu nhập khẩu")]
        [HttpPost]
        [Route("xac-nhan-api7")]
        public async Task<Response<bool>> ApproveAPI7(CreateNguyenLieuDto createNguyenLieuDto)
        {
            return await _dataManagerRepository.ApproveAPI7(createNguyenLieuDto);
        }
        [RoleDescription("Xác nhận Báo cáo khác cho TMĐMKD")]
        [HttpPost]
        [Route("xac-nhan-api8")]
        public async Task<Response<bool>> ApproveAPI8(CreateKhacDto createKhacDto)
        {
            return await _dataManagerRepository.ApproveAPI8(createKhacDto);
        }
        [RoleDescription("Xác nhận Quỹ bình ổn giá xăng dầu")]
        [HttpPost]
        [Route("xac-nhan-api9")]
        public async Task<Response<bool>> ApproveAPI9(CreateQuyBinhOnGiumDto createQuyBinhOnGiumDto)
        {
            return await _dataManagerRepository.ApproveAPI9(createQuyBinhOnGiumDto);
        }
        
        [RoleDescription("Từ chối API")]
        [HttpPost]
        [Route("tu-choi-api")]
        public async Task<Response<bool>> RejectAsync([FromBody] RejectApiDto request)
        {
            return await _dataManagerRepository.RejectAsync(request);
        }

        #region ExportExcel
        [RoleDescription("Xuất dữ liệu Giấy đăng kí kinh doanh")]
        [HttpPost]
        [Route("export-api1")]
        public async Task<Response<string>> ExportExcelApi1Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api1.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi1Async(request);
                var tinh = await _unitOfWork.GetRepository<Locality>().GetByExpression(n => n.Type == LocalityType.Tinh).ToListAsync();
                var huyen = await _unitOfWork.GetRepository<Locality>().GetByExpression(n => n.Type == LocalityType.Huyen).ToListAsync();
                var xa = await _unitOfWork.GetRepository<Locality>().GetByExpression(n => n.Type == LocalityType.Xa).ToListAsync();
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();

                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = item.TenDoanhNghiep;
                            workSheet.Cells[i, 3].Value = item.SoDkkd;
                            workSheet.Cells[i, 4].Value = item.MaSoThue;
                            workSheet.Cells[i, 5].Value = item.DiaChi;
                            workSheet.Cells[i, 6].Value = xa.FirstOrDefault(n => n.Id == item.DiaChiXa)?.Title;
                            workSheet.Cells[i, 7].Value = huyen.FirstOrDefault(n => n.Id == item.DiaChiHuyen)?.Title;
                            workSheet.Cells[i, 8].Value = tinh.FirstOrDefault(n => n.Id == item.DiaChiTinh)?.Title;
                            workSheet.Cells[i, 9].Value = item.SoDienThoai;
                            workSheet.Cells[i, 10].Value = item.NguoiDaiDienTen;
                            workSheet.Cells[i, 11].Value = item.NguoiDaiDienDob;
                            workSheet.Cells[i, 12].Value = item.NguoiDaiDienCccd;
                            workSheet.Cells[i, 13].Value = item.NguoiDaiDienSdt;
                            workSheet.Cells[i, 14].Value = item.ThoiGianCapNhat.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 15].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 15].Style.WrapText = true;

                        string newFileName = $"api1_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api1.xlsx"));
            }
            catch(Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api1.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Hệ thống phân phối")]
        [HttpPost]
        [Route("export-api2")]
        public async Task<Response<string>> ExportExcelApi2Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api2.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi2Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();

                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuongCha)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuongCon)?.TenDoiTuong;
                            workSheet.Cells[i, 3].Value = item.TenHeThongPhanPhoi;
                            workSheet.Cells[i, 4].Value = item.DiaChi;
                            workSheet.Cells[i, 5].Value = item.DienThoai;
                            workSheet.Cells[i, 6].Value = CommonFunction.GetLoaiDoiTuong(item.LoaiDoiTuongCon ?? 0);
                            workSheet.Cells[i, 7].Value = CommonFunction.GetLoaiSoHuu(item.LoaiSoHuu ?? 0);
                            workSheet.Cells[i, 8].Value = item.ThoiGianCapNhat.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 8].Style.WrapText = true;

                        string newFileName = $"api2_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api2.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api2.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Tình hình sản xuất xăng dầu")]
        [HttpPost]
        [Route("export-api3")]
        public async Task<Response<string>> ExportExcelApi3Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api3.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi3Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = chungLoai.FirstOrDefault(n => n.MaChungLoai == item.ChungLoai)?.TenChungLoai;
                            workSheet.Cells[i, 3].Value = item.LuongSanXuat;
                            workSheet.Cells[i, 4].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 5].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 5].Style.WrapText = true;

                        string newFileName = $"api4_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api3.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api3.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Tình hình tiêu thụ")]
        [HttpPost]
        [Route("export-api4")]
        public async Task<Response<string>> ExportExcelApi4Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api4.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi4Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = chungLoai.FirstOrDefault(n => n.MaChungLoai == item.ChungLoai)?.TenChungLoai;
                            workSheet.Cells[i, 3].Value = item.LuongBan;
                            workSheet.Cells[i, 4].Value = CommonFunction.GetLoaiBan(item.LoaiBan ?? 0);
                            workSheet.Cells[i, 5].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 6].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.WrapText = true;

                        string newFileName = $"api4_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api4.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api4.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Tình hình nhập xăng dầu")]
        [HttpPost]
        [Route("export-api5")]
        public async Task<Response<string>> ExportExcelApi5Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api5.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi5Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = chungLoai.FirstOrDefault(n => n.MaChungLoai == item.ChungLoai)?.TenChungLoai;
                            workSheet.Cells[i, 3].Value = item.LuongNhap;
                            workSheet.Cells[i, 4].Value = CommonFunction.GetLoaiNhap(item.LoaiNhap ?? 0);
                            workSheet.Cells[i, 5].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 6].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.WrapText = true;

                        string newFileName = $"api5_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api5.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api5.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Tình hình tồn kho")]
        [HttpPost]
        [Route("export-api6")]
        public async Task<Response<string>> ExportExcelApi6Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api6.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi6Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = chungLoai.FirstOrDefault(n => n.MaChungLoai == item.ChungLoai)?.TenChungLoai;
                            workSheet.Cells[i, 3].Value = item.LuongTon;
                            workSheet.Cells[i, 4].Value = CommonFunction.GetVungMien(item.VungMien ?? 0);
                            workSheet.Cells[i, 5].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 6].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.WrapText = true;

                        string newFileName = $"api6_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api6.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api6.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Lượng nguyên liệu nhập khẩu")]
        [HttpPost]
        [Route("export-api7")]
        public async Task<Response<string>> ExportExcelApi7Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api7.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi7Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = item.TenNguyenLieu;
                            workSheet.Cells[i, 3].Value = item.LuongNhap;
                            workSheet.Cells[i, 4].Value = CommonFunction.GetDonViTinh(Convert.ToInt32(item.DonViTinh));
                            workSheet.Cells[i, 5].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 6].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 6].Style.WrapText = true;

                        string newFileName = $"api7_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api7.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api7.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Báo cáo khác cho TMĐMKD")]
        [HttpPost]
        [Route("export-api8")]
        public async Task<Response<string>> ExportExcelApi8Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api8.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi8Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }

                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = chungLoai.FirstOrDefault(n => n.MaChungLoai == item.ChungLoai)?.TenChungLoai;
                            workSheet.Cells[i, 3].Value = item.LuongPhaChe;
                            workSheet.Cells[i, 4].Value = item.LuongTamNhapTaiXuat;
                            workSheet.Cells[i, 5].Value = item.LuongChuyenKhau;
                            workSheet.Cells[i, 6].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 7].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 7].Style.WrapText = true;

                        string newFileName = $"api8_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api8.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api8.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Quỹ bình ổn giá xăng dầu")]
        [HttpPost]
        [Route("export-api9")]
        public async Task<Response<string>> ExportExcelApi9Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api9.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi9Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 2].Value = item.SoDu;
                            workSheet.Cells[i, 3].Value = item.NgayBc.Value.ToString("dd/MM/yyyy HH:mm:ss");
                            workSheet.Cells[i, 4].Value = CommonFunction.GetApprovedStatusName(item.AprrovedStatus);
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 4].Style.WrapText = true;

                        string newFileName = $"api9_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api9.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api9.xlsx"));
            }
        }
        [RoleDescription("Xuất dữ liệu Dự kiến nhập kỳ tới")]
        [HttpPost]
        [Route("export-api10")]
        public async Task<Response<string>> ExportExcelApi10Async([FromBody] GetListApiRequestDto request)
        {
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "Template", "api10.xlsx");
            try
            {
                var data = await _dataManagerRepository.ExportExcelApi10Async(request);
                var doiTuong = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().ToListAsync();
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();
                if (data?.Count > 0)
                {
                    using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.None))
                    {
                        stream.Close();
                    }
                    FileInfo file = new FileInfo(filePath);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(file))
                    {
                        int i = 3;
                        var workSheet = package.Workbook.Worksheets[0];
                        foreach (var item in data)
                        {
                            workSheet.Cells[i, 1].Value = chungLoai.FirstOrDefault(n => n.MaChungLoai == item.ChungLoai)?.TenChungLoai;
                            workSheet.Cells[i, 2].Value = doiTuong.FirstOrDefault(n => n.MaDoiTuong == item.MaDoiTuong)?.TenDoiTuong;
                            workSheet.Cells[i, 3].Value = item.SoLuong;
                            workSheet.Cells[i, 4].Value = item.NhapKhau;
                            workSheet.Cells[i, 5].Value = item.MuaTuTNDMKhac;
                            workSheet.Cells[i, 6].Value = item.NhapNhaMayTrongNuoc;
                            workSheet.Cells[i, 7].Value = item.TuSXPC;
                            workSheet.Cells[i, 8].Value = item.NhapKhac;
                            workSheet.Cells[i, 9].Value = item.NgayBC.ToString("dd/MM/yyyy HH:mm:ss");
                            i++;
                        }
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Right.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Top.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Bottom.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Left.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.Border.Right.Color.SetColor(Color.Black);
                        workSheet.Cells[3, 1, (data.Count + 2), 9].Style.WrapText = true;

                        string newFileName = $"api10_{DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")}.xlsx";
                        string outputPath = Path.Combine(_webHostEnvironment.WebRootPath, "Temps", newFileName);
                        FileInfo fi = new FileInfo(outputPath);
                        package.SaveAs(fi);
                        return Response<string>.CreateSuccessResponse($"Temps/{newFileName}");
                    }
                }
                return Response<string>.CreateSuccessResponse(Path.Combine("Template", "api10.xlsx"));
            }
            catch (Exception ex)
            {
                return Response<string>.CreateErrorResponse(Path.Combine("Template", "api10.xlsx"));
            }
        }
        #endregion

        #region ImportExcel
        //[RoleDescription("Nhập dữ liệu từ file Excel")]
        [AuthenticationOnly]
        [HttpPost]
        [Route("import-api2")]
        public async Task<Response<bool>> ImportApi2Async()
        {
            return await _dataManagerRepository.ImportApi2Async();
        }
        #endregion
        [RoleDescription("Xóa API")]
        [HttpPost]
        [Route("xoa-api")]
        public async Task<Response<bool>> DeleteAsync([FromBody] RejectApiDto request)
        {
            return await _dataManagerRepository.DeleteAsync(request);
        }
    }
}
