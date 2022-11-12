using AutoMapper;
using BusinessLogic.Contracts;
using BusinessLogic.Hubs;
using BusinessLogic.UnitOfWork;
using Common;
using Common.Const;
using Common.Dtos;
using Common.Dtos.WarningSystemDtos;
using DataAccess;
using Microsoft.AspNetCore.SignalR;
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
    public class WarningSystemRepository : IWarningSystemRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogRepository _logRepository;
        private readonly IMapper _mapper;
        private readonly IConfigRepository _configRepository;
        public WarningSystemRepository(IUnitOfWork unitOfWork, ILogRepository logRepository, IMapper mapper, IConfigRepository configRepository)
        {
            _unitOfWork = unitOfWork;
            _logRepository = logRepository;
            _mapper = mapper;
            _configRepository = configRepository;
        }

        public async Task<Response<object>> InputedWarningAsync()
        {
            // th chưa nhập dl
            var conf = (await _configRepository.GetExpireDayAsync()).ResponseData;
            var existsApi3 = await _unitOfWork.GetAsQueryable<SanXuat>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();
            var existsApi4 = await _unitOfWork.GetAsQueryable<TieuThu>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();
            var existsApi5 = await _unitOfWork.GetAsQueryable<Nhap>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();
            var existsApi6 = await _unitOfWork.GetAsQueryable<TonKho>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();
            var existsApi7 = await _unitOfWork.GetAsQueryable<NguyenLieu>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();
            var existsApi8 = await _unitOfWork.GetAsQueryable<Khac>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();
            var existsApi9 = await _unitOfWork.GetAsQueryable<QuyBinhOnGium>().Where(n => n.NgayBc.Value.Date >= conf.PrevWeekStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevWeekEnd.Value.Date).Select(n => n.MaDoiTuong).Distinct().ToListAsync();

            return Response<object>.CreateSuccessResponse(await _unitOfWork.GetAsQueryable<DoiTuongQuanLy>().OrderBy(n => n.MaDoiTuong).ThenBy(n => n.ParentId).Select(n => new
            {
                n.MaDoiTuong,
                n.TenDoiTuong,
                existsApi3 = existsApi3.Contains(n.MaDoiTuong),
                existsApi4 = existsApi4.Contains(n.MaDoiTuong),
                existsApi5 = existsApi5.Contains(n.MaDoiTuong),
                existsApi6 = existsApi6.Contains(n.MaDoiTuong),
                existsApi7 = existsApi7.Contains(n.MaDoiTuong),
                existsApi8 = existsApi8.Contains(n.MaDoiTuong),
                existsApi9 = existsApi9.Contains(n.MaDoiTuong),
            }).ToListAsync());
        }
        public async Task<Response<object>> IllogicalDataAsync()
        {
            var conf = (await _configRepository.GetExpireDayAsync()).ResponseData;
            var inputQuery = _unitOfWork.GetAsQueryable<Nhap>()
                .Where(n => n.NgayBc.Value.Date >= conf.PrevMonthStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevMonthEnd.Value.Date && n.AprrovedStatus == ApprovedStatus.Approve && n.MaDoiTuong.HasValue)
                .GroupBy(n => n.MaDoiTuong)
                .Select(n => new
                {
                    MaDoiTuong = n.Key,
                    Nhap = n.Select(m => m.LuongNhap).Sum()
                });
            var consumeQuery = _unitOfWork.GetAsQueryable<TieuThu>()
                .Where(n => n.NgayBc.Value.Date >= conf.PrevMonthStart.Value.Date && n.NgayBc.Value.Date <= conf.PrevMonthEnd.Value.Date && n.AprrovedStatus == ApprovedStatus.Approve && n.MaDoiTuong.HasValue)
                .GroupBy(n => n.MaDoiTuong)
                .Select(n => new
                {
                    MaDoiTuong = n.Key,
                    LuongBan = n.Select(m => m.LuongBan).Sum()
                });
            var query = await (from d in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>() 
                        join i in inputQuery on d.MaDoiTuong equals i.MaDoiTuong into g1 from g1D in g1.DefaultIfEmpty()
                        join c in consumeQuery on d.MaDoiTuong equals c.MaDoiTuong into g2 from g2D in g2.DefaultIfEmpty()
                        select new
                        {
                            d.MaDoiTuong,
                            d.TenDoiTuong,
                            Nhap = g1D != null ? g1D.Nhap : 0,
                            TieuThu = g2D != null ? g2D.LuongBan : 0
                        }).ToListAsync();
            return Response<object>.CreateSuccessResponse(query);
        }
        public async Task<Response<object>> DistributionSystemDataAsync()
        {
            var parents = _unitOfWork.GetAsQueryable<DoiTuongQuanLy>().Where(n => n.LoaiDoiTuong == (int)LoaiDoiTuongEnum.Dmsx || n.LoaiDoiTuong == (int)LoaiDoiTuongEnum.Dmkd);
            var childs = _unitOfWork.GetAsQueryable<HeThongPhanPhoi>().Where(n => parents.Any(m => m.MaDoiTuong == n.MaDoiTuongCha));
            
            // test 
            var tnpp = _unitOfWork.GetAsQueryable<DoiTuongQuanLy>().Where(n => n.LoaiDoiTuong == (int)LoaiDoiTuongEnum.Tnpp);
            var daily = _unitOfWork.GetAsQueryable<HeThongPhanPhoi>().Where(n => tnpp.Any(m => m.MaDoiTuong == n.MaDoiTuongCha) && n.LoaiDoiTuongCon == (int)LoaiDoiTuongEnum.Dlbl);
            var cuaHangThuocDaiLy = _unitOfWork.GetAsQueryable<HeThongPhanPhoi>().Where(n => daily.Any(m => m.MaDoiTuongCon == n.MaDoiTuongCha) && n.LoaiDoiTuongCon == (int)LoaiDoiTuongEnum.Ch);
            var cuaHangTrucThuoc = _unitOfWork.GetAsQueryable<HeThongPhanPhoi>().Where(n => tnpp.Any(m => m.MaDoiTuong == n.MaDoiTuongCha) && n.LoaiDoiTuongCon == (int)LoaiDoiTuongEnum.Ch);

            var tnppResult = await tnpp.Select(n => new
            {
                n.MaDoiTuong,
                n.TenDoiTuong,
                n.LoaiDoiTuong,
                cuaHangTrucThuoc = cuaHangTrucThuoc.Where(m => m.MaDoiTuongCha == n.MaDoiTuong).Count(),
                cuaHang = cuaHangThuocDaiLy.Where(m => daily.Where(s => s.MaDoiTuongCha == n.MaDoiTuong).Select(s => s.MaDoiTuongCon).Contains(m.MaDoiTuongCha)).Count()
            }).ToListAsync();
            var tndmResult = await parents.Select(n => new
            {
                n.MaDoiTuong,
                n.TenDoiTuong,
                n.LoaiDoiTuong,
                Daily = childs.Where(m => m.LoaiDoiTuongCon == (int)LoaiDoiTuongEnum.Dlbl && m.MaDoiTuongCha == n.MaDoiTuong).Count(),
                Cuahang = childs.Where(m => m.LoaiDoiTuongCon == (int)LoaiDoiTuongEnum.Ch && m.MaDoiTuongCha == n.MaDoiTuong).Count(),
            }).ToListAsync();

            return Response<object>.CreateSuccessResponse(new
            {
                tnpp = tnppResult,
                tndm = tndmResult
            });
        }
        public async Task<Response<List<DuLieuXangDauViewsDto>>> ListDuLieuXangDauAsync()
        {
            try
            {


                var ketqua1 = from dt in _unitOfWork.GetRepository<Nhap>().GetAll()
                              group dt by dt.MaDoiTuong into g
                              select new
                              {
                                  madoituong = g.Key,
                                  luongnhap = g.Select(n => n.LuongNhap).Sum(),
                              };

                var ketqua2 = from dt in _unitOfWork.GetRepository<TieuThu>().GetAll()
                              group dt by dt.MaDoiTuong into g
                              select new
                              {
                                  madoituong = g.Key,
                                  luongban = g.Select(n => n.LuongBan).Sum(),
                              };

                var result = from dt in _unitOfWork.GetRepository<DoiTuongQuanLy>().GetAll()
                             join n in ketqua1 on dt.MaDoiTuong equals n.madoituong into item
                             from SoLuongNhap in item.DefaultIfEmpty()
                             select new SoLuongNhap
                             {
                                 MaDoiTuong = dt.MaDoiTuong,
                                 TenDoiTuong = dt.TenDoiTuong,
                                 LuongNhap = SoLuongNhap.luongnhap,
                             };

                var listData = (from rs in result
                             join kq2 in ketqua2 on rs.MaDoiTuong equals kq2.madoituong into item
                             from dulieu in item.DefaultIfEmpty()
                             select new DuLieuXangDauViewsDto
                             {
                                 TenDoiTuong = rs.TenDoiTuong,
                                 LuongBan = dulieu.luongban,
                                 LuongNhap = rs.LuongNhap,
                             }).ToList();
                return Response<List<DuLieuXangDauViewsDto>>.CreateSuccessResponse(listData);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<List<DuLieuXangDauViewsDto>>.CreateErrorResponse(ex);
            }
        }

        public async Task<Response<object>> DeviantPriceDataAsync()
        {
            try
            {
                var doanhNghiep = _unitOfWork.GetAsQueryable<GiaBanXangDauDoanhNghiep>().Where(n => n.MaDoiTuong != 0);
                var query = await (from dt in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                             join d in doanhNghiep on dt.MaDoiTuong equals d.MaDoiTuong
                             join c in _unitOfWork.GetAsQueryable<ChungLoai>() on d.MaChungLoai equals c.MaChungLoai
                             select new
                             {
                                 dt.MaDoiTuong,
                                 dt.TenDoiTuong,
                                 d.MaChungLoai,
                                 c.TenChungLoai,
                                 d.Gia,
                                 d.CreatedDate,
                                 d.DonViTinh
                             }).ToListAsync();
                var result = query.GroupBy(n => new
                {
                    n.MaDoiTuong,
                    n.TenDoiTuong
                }).Select(n => new
                {
                    n.Key.MaDoiTuong,
                    n.Key.TenDoiTuong,
                    ChungLoai = n.Select(m => new
                    {
                        m.MaChungLoai,
                        m.TenChungLoai,
                        m.Gia,
                        m.DonViTinh
                    }).OrderBy(n => n.MaChungLoai)
                });

                return Response<object>.CreateSuccessResponse(result);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<object>.CreateErrorResponse(ex);
            }
        }
        public async Task<Response<object>> TotalAllocationsDataAsync()
        {
            try
            {
                int year = DateTime.Now.Year;
                var query = await (from d in _unitOfWork.GetAsQueryable<DoiTuongQuanLy>()
                                   join t in _unitOfWork.GetAsQueryable<TongNguonPhanGiao>() on d.MaDoiTuong equals t.MaDoiTuong
                                   join cl in _unitOfWork.GetAsQueryable<ChungLoai>() on t.MaChungLoai equals cl.MaChungLoai
                                   where t.Nam == year
                                   select new
                                   {
                                       d.MaDoiTuong,
                                       d.TenDoiTuong,
                                       t.MaChungLoai,
                                       cl.TenChungLoai,
                                       t.SoLuong,
                                       t.Nam
                                   }).ToListAsync();
                var thucTe = await _unitOfWork.GetAsQueryable<Nhap>().Where(n => n.NgayBc.Value.Year == year && n.AprrovedStatus == ApprovedStatus.Approve)
                    .GroupBy(n => new { n.MaDoiTuong, n.ChungLoai})
                    .Select(n => new
                    {
                        n.Key.MaDoiTuong,
                        n.Key.ChungLoai,
                        LuongNhap = n.Select(m => m.LuongNhap).Sum()
                    })
                    .ToListAsync();
                var join = (from q in query
                            join t in thucTe on q.MaDoiTuong equals t.MaDoiTuong into g
                            from gr in g.DefaultIfEmpty()
                            select new
                            {
                                q.MaDoiTuong,
                                q.TenDoiTuong,
                                q.MaChungLoai,
                                q.TenChungLoai,
                                q.SoLuong,
                                LuongNhap = gr != null ? gr.LuongNhap : 0,
                                q.Nam
                            });

                var response = join.GroupBy(n => new
                            {
                                n.MaDoiTuong,
                                n.TenDoiTuong
                            })
                            .Select(n => new
                            {
                                n.Key.MaDoiTuong,
                                n.Key.TenDoiTuong,
                                ChungLoai = n.Select(m => new
                                {
                                    m.MaChungLoai,
                                    m.TenChungLoai,
                                    m.SoLuong,
                                    m.LuongNhap,
                                    m.Nam
                                })
                            });
                return Response<object>.CreateSuccessResponse(response);
            }
            catch (Exception ex)
            {
                await _logRepository.ErrorAsync(ex.ToString());
                return Response<object>.CreateErrorResponse(ex);
            }
        }
    }
}
