using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class DuKienNhapDto
    {
        public long Id { get; set; }
        public string TenDoiTuong { get; set; }
        public int? MaDoiTuong { get; set; }
        public int? ChungLoai { get; set; }
        public double? SoLuong { get; set; }
        public double? NhapKhau { get; set; }
        public double? NhapNhaMayTrongNuoc { get; set; }
        public double? MuaTuTNDMKhac { get; set; }
        public double? TuSXPC { get; set; }
        public double? NhapKhac { get; set; }
        public DateTime NgayBC { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
