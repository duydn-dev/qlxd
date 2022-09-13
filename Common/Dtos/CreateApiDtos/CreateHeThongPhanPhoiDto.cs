using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.CreateApiDtos
{
    public class CreateHeThongPhanPhoiDto
    {
        public int? MaDoiTuongCha { get; set; }
        public int MaDoiTuongCon { get; set; }
        public string TenHeThongPhanPhoi { get; set; }
        public string DiaChi { get; set; }
        public string DienThoai { get; set; }
        public int? LoaiDoiTuongCon { get; set; }
        public int? LoaiSoHuu { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public long Id { get; set; }
        public int AprrovedStatus { get; set; }
        public string MD5 { get; set; }
        public int MaPhienBan { get; set; }
        public int Version { get; set; }
        public DateTime? AprrovedDate { get; set; }
        public string RejectMessage { get; set; }
        public Guid? AprrovedBy { get; set; }
        public bool IsActive { get; set; }
    }
}
