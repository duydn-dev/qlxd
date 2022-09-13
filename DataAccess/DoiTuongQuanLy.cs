using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class DoiTuongQuanLy
    {
        public int MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public int? LoaiDoiTuong { get; set; }
        public string ToBen { get; set; }
        public string WhiteListIp { get; set; }
        public int ParentId { get; set; }
    }
}
