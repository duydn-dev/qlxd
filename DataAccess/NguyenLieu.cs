using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class NguyenLieu
    {
        public int? MaDoiTuong { get; set; }
        public string TenNguyenLieu { get; set; }
        public double? LuongNhap { get; set; }
        public string DonViTinh { get; set; }
        public DateTime? NgayBc { get; set; }
        public long Id { get; set; }
        public int AprrovedStatus { get; set; }
        public Guid? AprrovedBy { get; set; }
        public string MD5 { get; set; }
        public int MaPhienBan { get; set; }
        public int Version { get; set; }
        public DateTime? AprrovedDate { get; set; }
        public string RejectMessage { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
