using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class Company
    {
        public int CompanyId { get; set; }
        public string NameCp { get; set; }
        public string SoGiayDangKyKinhDoanh { get; set; }
        public string MaSoThue { get; set; }
        public string DiaChi { get; set; }
        public string SdtlienHe { get; set; }
        public string TenNguoiDaiDienPhapLuat { get; set; }
        public string NgayThangNamSinhNguoiDaiDienPhapLuat { get; set; }
        public string CmtOrCccdnguoiDaiDienPhapLuat { get; set; }
        public string SdtlienHeNguoiDaiDienPhapLuat { get; set; }
        public string Website { get; set; }
        public int? Status { get; set; }
        public string Note { get; set; }
        public string Email { get; set; }
        public string Logo { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public int? IdZone { get; set; }
        public int? TypeId { get; set; }
        public int? Mien { get; set; }
    }
}
