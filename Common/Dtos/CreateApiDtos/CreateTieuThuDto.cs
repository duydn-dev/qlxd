using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.CreateApiDtos
{
    public class CreateTieuThuDto
    {
        public int? MaDoiTuong { get; set; }
        public int? ChungLoai { get; set; }
        public double? LuongBan { get; set; }
        public DateTime? NgayBc { get; set; }
        public int? LoaiBan { get; set; }
        public long Id { get; set; }
        public int AprrovedStatus { get; set; }
        public string MD5 { get; set; }
        public int MaPhienBan { get; set; }
        public int Version { get; set; }
        public DateTime? AprrovedDate { get; set; }
        public string RejectMessage { get; set; }
        public Guid? AprrovedBy { get; set; }
        public bool IsDelete { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
