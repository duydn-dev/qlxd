using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class HeThongPhanPhoiDto
    {
        public int MaDoiTuongCha { get; set; }
        public string DoiTuongCha { get; set; }
        public int MaDoiTuongCon { get; set; }
        public string DoiTuongCon { get; set; }
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
