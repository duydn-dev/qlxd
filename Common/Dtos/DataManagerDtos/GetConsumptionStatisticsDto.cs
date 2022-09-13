using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class GetConsumptionStatisticsDto
    {
        public string GasolineName { get; set; }
        public float TotalNhap { get; set; }
        public float TotalTieuThu { get; set; }
        public float TotalTon { get; set; }
        public float TotalTonBac { get; set; }
        public float TotalTonTrung { get; set; }
        public float TotalTonNam { get; set; }
    }
    public class GetConsumptionStatisticsRequestDto
    {
        public DateTime? FromDate { get; set; } 
        public DateTime? ToDate { get; set; } 
        public int DoiTuongId { get; set; }
    }
    public class BinhOnGiaDto
    {
        public int MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public List<BinhOnGiaMonthDto> BinhOnGiaMonthDtos { get; set; } 
    }
    public class BinhOnGiaMonthDto
    {
        public DateTime? NgayBc { get; set; }
        public double? SoDu { get; set; }
    }
    public class GetByIdRequestDto
    {
        public int Type { get; set; }
        public int Id { get; set; }
    }
}
