using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DoiTuongQuanLyDtos
{
    public class DoiTuongQuanLyViewDto
    {
        public int MaDoiTuong { get; set; }
        public string TenDoiTuong { get; set; }
        public int? LoaiDoiTuong { get; set; }
        public string ToBen { get; set; }
        public string WhiteListIp { get; set; }
        public int ParentId { get; set; }
    }
}
