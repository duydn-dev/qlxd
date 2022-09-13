using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.WarningSystemDtos
{
    public class WarningSystemViewsDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public string UserName { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
    public class DuLieuXangDauViewsDto
    {
        public string TenDoiTuong { get; set; }
        public double? LuongNhap { get; set; }
        public double? LuongBan { get; set; }
    }
    public class SoLuongNhap
    {
        public int MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public double? LuongNhap { get; set; }
    }
}
