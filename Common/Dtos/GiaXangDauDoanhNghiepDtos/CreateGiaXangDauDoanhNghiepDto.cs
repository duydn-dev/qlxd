using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.GiaXangDauDoanhNgiepDtos
{
    public class CreateGiaXangDauDoanhNghiepDto
    {
        public long Id { get; set; }
        public int MaChungLoai { get; set; }
        public double Gia { get; set; }
        public int? MaDoiTuong { get; set; }
        public string DonViTinh { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
