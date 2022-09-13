using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.TongNguonPhanGiaoDtos
{
    public class TongNguonPhanGiaoViewsDto
    {
        public long Id { get; set; }
        public string TenChungLoai { get; set; }
        public int? MaChungLoai { get; set; }
        public int? MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public double? SoLuong { get; set; }
        public int? Nam { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
