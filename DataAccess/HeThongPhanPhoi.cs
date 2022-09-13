using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class HeThongPhanPhoi
    {
        public int MaDoiTuongCha { get; set; }
        public int MaDoiTuongCon { get; set; }
        public string TenHeThongPhanPhoi { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public int? LoaiDoiTuongCon { get; set; }
        public int? LoaiSoHuu { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public long Id { get; set; }
        public bool IsActive { get; set; }
    }
}
