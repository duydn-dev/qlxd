using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class GiayDangKyKinhDoanhVer
    {
        public int MaDoiTuong { get; set; }
        public string TenDoanhNghiep { get; set; }
        public string SoDkkd { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChi { get; set; }
        public int? DiaChiXa { get; set; }
        public int? DiaChiHuyen { get; set; }
        public int? DiaChiTinh { get; set; }
        public string SoDienThoai { get; set; }
        public string NguoiDaiDienTen { get; set; }
        public string NguoiDaiDienDob { get; set; }
        public string NguoiDaiDienCccd { get; set; }
        public string NguoiDaiDienSdt { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public int? Ver { get; set; }
        public long Id { get; set; }
    }
}
