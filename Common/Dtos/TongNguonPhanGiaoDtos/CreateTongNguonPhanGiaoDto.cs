using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.TongNguonPhanGiaoDtos
{
    public class CreateTongNguonPhanGiaoDto
    {
        public long Id { get; set; }
        public int? MaDoiTuong { get; set; }
        public int MaChungLoai { get; set; }
        public double SoLuong { get; set; }
        public int Nam { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
