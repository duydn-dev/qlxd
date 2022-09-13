using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class CauHinh
    {
        public int MaChungLoai { get; set; }
        public int? DonViTinh { get; set; }
        public string TenDonVi { get; set; }
        public DateTime? NgayTao { get; set; }
        public long Id { get; set; }
    }
}
