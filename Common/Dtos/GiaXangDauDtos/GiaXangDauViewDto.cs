using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.GiaXangDauDtos
{
    public class GiaXangDauViewDto
    {
        public long Id { get; set; }
        public int? MaChungLoai { get; set; }
        public string TenChungLoai { get; set; }
        public double? Gia { get; set; }
        public int VungMien { get; set; }
        public DateTime? DotBienDong { get; set; }
        public DateTime? NgayCapNhat { get; set; }
        public int TrangThai { get; set; }
        public string DonViTinh { get; set; }
    }
}
