using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Const;
using Common.Dtos;
using Common.Dtos.DataManagerDtos;
using Common.Dtos.DoiTuongQuanLyDtos;
using DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Const.CommonConstant;

namespace BusinessLogic.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public DashboardRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<Response<GetDashBoardDtqlDto>> GetList()
        {
            try
            {
                //thêm Loaidoituong trong appsettings.json , thêm dto
                GetDashBoardDtqlDto response = new GetDashBoardDtqlDto();
                response.Tnpp = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(x => x.LoaiDoiTuong == Convert.ToInt32(_configuration.GetSection("LoaiDoiTuong").GetSection("TNPP").Value)).CountAsync();
                response.Dmkd = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(x => x.LoaiDoiTuong == Convert.ToInt32(_configuration.GetSection("LoaiDoiTuong").GetSection("DMKD").Value)).CountAsync();
                response.Dmsx = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(x => x.LoaiDoiTuong == Convert.ToInt32(_configuration.GetSection("LoaiDoiTuong").GetSection("DMSX").Value)).CountAsync();
                response.Dlbl = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(x => x.LoaiDoiTuong == Convert.ToInt32(_configuration.GetSection("LoaiDoiTuong").GetSection("DLBL").Value)).CountAsync();
                response.Tdl = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(x => x.LoaiDoiTuong == Convert.ToInt32(_configuration.GetSection("LoaiDoiTuong").GetSection("TDL").Value)).CountAsync();
                response.Nqbl = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().Where(x => x.LoaiDoiTuong == Convert.ToInt32(_configuration.GetSection("LoaiDoiTuong").GetSection("NQBL").Value)).CountAsync();
                response.Sdvdkn = await _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll().CountAsync();
                var api3 = _unitOfWork.GetAsQueryable<SanXuat>();
                var api4 = _unitOfWork.GetAsQueryable<TieuThu>();
                var api5 = _unitOfWork.GetAsQueryable<Nhap>();
                var api6 = _unitOfWork.GetAsQueryable<TonKho>();
                var api7 = _unitOfWork.GetAsQueryable<NguyenLieu>();
                var api8 = _unitOfWork.GetAsQueryable<Khac>();
                var api9 = _unitOfWork.GetAsQueryable<QuyBinhOnGium>();
                response.Tsgd = await api3.CountAsync() + await api4.CountAsync() + await api5.CountAsync() + await api6.CountAsync() + await api7.CountAsync() + await api8.CountAsync() + await api9.CountAsync();
                response.Sgddxt = 
                    await api3.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync() +
                    await api4.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync() + 
                    await api5.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync() +
                    await api6.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync() +
                    await api7.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync() +
                    await api8.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync() +
                    await api9.Where(x => x.AprrovedStatus == ApprovedStatus.Approve).CountAsync();
                response.Tlxt = Math.Round((Convert.ToDouble(response.Sgddxt) / Convert.ToDouble(response.Tsgd)) * 100);
                return Response<GetDashBoardDtqlDto>.CreateSuccessResponse(response);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<GetDashBoardDtqlDto>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<object>> GetListStatisticalGasAsync(GetConsumptionStatisticsRequestDto request)
        {
            try
            {
                int year = DateTime.Now.Year;   
                if(request.FromDate.HasValue) year = request.FromDate.Value.Year;
                // Chủng loại
                var chungLoai = await _unitOfWork.GetRepository<ChungLoai>().GetAll().ToListAsync();

                var commonNhap = (from c in _unitOfWork.GetRepository<ChungLoai>().GetAll()
                                  join n in _unitOfWork.GetRepository<Nhap>().GetAll().Where(n => n.AprrovedStatus == ApprovedStatus.Approve) on c.MaChungLoai equals n.ChungLoai
                                  where (!request.FromDate.HasValue || n.NgayBc.Value.Date >= request.FromDate)
                                         && (!request.ToDate.HasValue || n.NgayBc.Value.Date <= request.ToDate)
                                         && (request.DoiTuongId == 0 || n.MaDoiTuong == request.DoiTuongId)
                                  select new
                                  {
                                      c.MaChungLoai,
                                      c.TenChungLoai,
                                      n.MaDoiTuong,
                                      ToTalNhap = n.LuongNhap,
                                  });

                var commonTieuThu = (from c in _unitOfWork.GetRepository<ChungLoai>().GetAll()
                                     join n in _unitOfWork.GetRepository<TieuThu>().GetAll().Where(n => n.AprrovedStatus == ApprovedStatus.Approve) on c.MaChungLoai equals n.ChungLoai
                                     where (!request.FromDate.HasValue || n.NgayBc.Value.Date >= request.FromDate)
                                            && (!request.ToDate.HasValue || n.NgayBc.Value.Date <= request.ToDate)
                                            && (request.DoiTuongId == 0 || n.MaDoiTuong == request.DoiTuongId)
                                     select new
                                     {
                                         c.MaChungLoai,
                                         c.TenChungLoai,
                                         n.MaDoiTuong,
                                         ToTalNhap = n.LuongBan,
                                     });

                var commonTon = (from c in _unitOfWork.GetRepository<ChungLoai>().GetAll()
                                 join n in _unitOfWork.GetAsQueryable<TonKho>().Where(n => n.AprrovedStatus == ApprovedStatus.Approve) on c.MaChungLoai equals n.ChungLoai
                                 where (!request.FromDate.HasValue || n.NgayBc.Value.Date >= request.FromDate)
                                        && (!request.ToDate.HasValue || n.NgayBc.Value.Date <= request.ToDate)
                                        && (request.DoiTuongId == 0 || n.MaDoiTuong == request.DoiTuongId)
                                 select new
                                 {
                                     c.MaChungLoai,
                                     c.TenChungLoai,
                                     n.VungMien,
                                     ToTalTon = n.LuongTon,
                                     n.MaDoiTuong
                                 });

                #region tổng hợp
                // lấy số lượng nhập
                var queryNhap = await commonNhap.GroupBy(n => new { n.MaChungLoai, n.TenChungLoai })
                                .Select(n => new
                                {
                                    n.Key.MaChungLoai,
                                    n.Key.TenChungLoai,
                                    TotalNhap = n.Select(m => m.ToTalNhap).Sum()
                                }).ToListAsync();

                // lấy số lượng tiêu thụ
                var queryTieuThu = await commonTieuThu.GroupBy(n => new { n.MaChungLoai, n.TenChungLoai })
                                .Select(n => new
                                {
                                    n.Key.MaChungLoai,
                                    n.Key.TenChungLoai,
                                    TotalTieuThu = n.Select(m => m.ToTalNhap).Sum()
                                }).ToListAsync();

                // lấy số tổng số lượng tồn


                var queryTonKho = await commonTon
                                .GroupBy(n => new { n.MaChungLoai, n.TenChungLoai })
                                .Select(n => new
                                {
                                    n.Key.MaChungLoai,
                                    n.Key.TenChungLoai,
                                    TotalTon = n.Select(m => m.ToTalTon).Sum()
                                }).ToListAsync();

                var queryTonVungMien = await commonTon
                                        .GroupBy(n => new { n.MaChungLoai, n.TenChungLoai, n.VungMien })
                                        .OrderBy(n => n.Key.MaChungLoai)
                                        .ThenBy(n => n.Key.TenChungLoai)
                                        .ThenBy(n => n.Key.VungMien)
                                        .Select(n => new
                                        {
                                            n.Key.MaChungLoai,
                                            n.Key.TenChungLoai,
                                            n.Key.VungMien,
                                            TotalTonTheoMien = n.Select(m => m.ToTalTon).Sum()
                                        })
                                        .ToListAsync();

                var query = from c in chungLoai
                            join n in queryNhap on c.MaChungLoai equals n.MaChungLoai
                            into g1
                            from gr1 in g1.DefaultIfEmpty()

                            join b in queryTieuThu on c.MaChungLoai equals b.MaChungLoai
                            into g2
                            from gr2 in g2.DefaultIfEmpty()

                            join t in queryTonKho on c.MaChungLoai equals t.MaChungLoai
                            into g3
                            from gr3 in g3.DefaultIfEmpty()
                            select new
                            {
                                c.MaChungLoai,
                                c.TenChungLoai,
                                TotalNhap = gr1 != null ? gr1.TotalNhap : 0,
                                TotalTieuThu = gr2 != null ? gr2.TotalTieuThu : 0,
                                ToTalTon = gr3 != null ? gr3.TotalTon : 0
                            };
                #endregion
                var querySanXuat = await (from c in _unitOfWork.GetRepository<ChungLoai>().GetAll()
                                          join n in _unitOfWork.GetRepository<SanXuat>().GetAll().Where(n => (!request.FromDate.HasValue || n.NgayBc.Value.Date >= request.FromDate)
                                              && (!request.ToDate.HasValue || n.NgayBc.Value.Date <= request.ToDate)
                                              && (request.DoiTuongId == 0 || n.MaDoiTuong == request.DoiTuongId)
                                              && n.AprrovedStatus == ApprovedStatus.Approve)                
                                          on c.MaChungLoai equals n.ChungLoai into g1 from g1D in g1.DefaultIfEmpty()
                                          select new
                                          {
                                              c.MaChungLoai,
                                              c.TenChungLoai,
                                              LuongSanXuat = g1D == null ? 0 : g1D.LuongSanXuat,
                                          })
                                    .GroupBy(n => new
                                    {
                                        n.MaChungLoai,
                                        n.TenChungLoai
                                    })
                                    .Select(n => new
                                    {
                                        n.Key.MaChungLoai,
                                        n.Key.TenChungLoai,
                                        TotalSanXuat = n.Select(m => m.LuongSanXuat).Sum()
                                    }).ToListAsync();

                var sanXuat = (from c in chungLoai
                               join n in queryNhap on c.MaChungLoai equals n.MaChungLoai
                               into g1
                               from gr1 in g1.DefaultIfEmpty()

                               join s in querySanXuat on c.MaChungLoai equals s.MaChungLoai
                               into g2
                               from gr2 in g2.DefaultIfEmpty()
                               select new
                               {
                                   c.MaChungLoai,
                                   c.TenChungLoai,
                                   TotalNhap = gr1 != null ? gr1.TotalNhap : 0,
                                   TotalSanXuat = gr2 != null ? gr2.TotalSanXuat : 0,
                               });

                var quyBinhOnGia = await (from c in _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll()
                                          join n in _unitOfWork.GetRepository<QuyBinhOnGium>().GetAll().Where(n => n.AprrovedStatus == ApprovedStatus.Approve) on c.MaDoiTuong equals n.MaDoiTuong
                                          where (!request.FromDate.HasValue || n.NgayBc.Value.Date >= request.FromDate)
                                                 && (!request.ToDate.HasValue || n.NgayBc.Value.Date <= request.ToDate)
                                                 && (request.DoiTuongId == 0 || n.MaDoiTuong == request.DoiTuongId)
                                          select new
                                          {
                                              c.MaDoiTuong,
                                              c.TenDoiTuong,
                                              n.NgayBc,
                                              n.SoDu,
                                          })
                                  .ToListAsync();

                var binhOnResult = quyBinhOnGia
                    .GroupBy(n => new { n.MaDoiTuong, n.TenDoiTuong })
                    .Select(n => new BinhOnGiaDto
                    {
                        MaDoiTuong = n.Key.MaDoiTuong,
                        TenDoiTuong = n.Key.TenDoiTuong,
                        BinhOnGiaMonthDtos = n.Select(m => new BinhOnGiaMonthDto
                        {
                            NgayBc = m.NgayBc,
                            SoDu = m.SoDu,
                        }).ToList(),
                    });

                var binhOnNamNay = _unitOfWork.GetRepository<QuyBinhOnGium>().GetByExpression(n => n.NgayBc.Value.Year == year);

                var binhOnListResut = new List<BinhOnGiaDto>();
                foreach (var item in binhOnResult)
                {
                    var binhOnLast = new BinhOnGiaDto();
                    binhOnLast.MaDoiTuong = item.MaDoiTuong;
                    binhOnLast.TenDoiTuong = item.TenDoiTuong;
                    binhOnLast.BinhOnGiaMonthDtos = new List<BinhOnGiaMonthDto>();
                    if (item.BinhOnGiaMonthDtos?.Count > 0)
                    {
                        for (int i = 1; i < 13; i++)
                        {
                            var existItem = await binhOnNamNay.Where(n => n.NgayBc.Value.Month == i && n.MaDoiTuong == item.MaDoiTuong).ToListAsync();
                            if(existItem?.Count <= 0)
                            {
                                binhOnLast.BinhOnGiaMonthDtos.Add(new BinhOnGiaMonthDto
                                {
                                    NgayBc = new DateTime(year, i, 1),
                                    SoDu = 0
                                });
                            }
                            else
                            {
                                binhOnLast.BinhOnGiaMonthDtos.Add(new BinhOnGiaMonthDto
                                {
                                    NgayBc = new DateTime(existItem.FirstOrDefault().NgayBc.Value.Year, i, 1),
                                    SoDu = existItem.Select(n => n.SoDu).Sum()
                                });
                            }
                        }
                    }
                    else
                    {
                        for (int i = 1; i < 13; i++)
                        {
                            item.BinhOnGiaMonthDtos.Add(new BinhOnGiaMonthDto
                            {
                                NgayBc = new DateTime(year, i, 1),
                                SoDu = 0
                            });
                        }
                    }
                    binhOnListResut.Add(binhOnLast);
                }

                dynamic respone = new
                {
                    thongKeSoLuong = query,
                    tonTheoVungMien = queryTonVungMien,
                    sanXuat = sanXuat,
                    sxTheoLoai = querySanXuat,
                    ttTheoLoai = queryTieuThu,
                    quyBinhOnGia = binhOnListResut.WhereIf(request.DoiTuongId != 0,n => n.MaDoiTuong == request.DoiTuongId)
                };
                return Response<object>.CreateSuccessResponse(respone);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex);
                return Response<object>.CreateErrorResponse(ex);
            }
        }
    }
}
