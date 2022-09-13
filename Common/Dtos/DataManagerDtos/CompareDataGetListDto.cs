using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class CompareDataGetListDto
    {
        public int? LoaiDoiTuong { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? MaDoiTuong1 { get; set; }  
        public int? MaDoiTuong2 { get; set; }  
    }
}
