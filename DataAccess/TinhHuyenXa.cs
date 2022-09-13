using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class TinhHuyenXa
    {
        public int TinhHuyenXaId { get; set; }
        public string TenTinhHuyenXa { get; set; }
        public int? Loai { get; set; }
        public DateTime? NgayTao { get; set; }
        public int? ParentId { get; set; }
    }
}
