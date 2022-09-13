using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class LoaiMatHang
    {
        public int LoaiMatHangId { get; set; }
        public double? TenMatHang { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? TrangThai { get; set; }
    }
}
