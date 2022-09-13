using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class ChungLoaiDto
    {
        public int MaChungLoai { get; set; }
        public string TenChungLoai { get; set; }
        public int? MaDonViTinh { get; set; }
        public string TenDonViTinh { get; set; }
        public DateTime? NgayTao { get; set; }
    }
}
