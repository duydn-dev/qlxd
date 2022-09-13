using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DoiTuongQuanLyDtos
{
    public class DoiTuongQuanLyGetPageDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TextSearch { get; set; }
        public int? LoaiDoiTuong { get; set; }

        public DoiTuongQuanLyGetPageDto()
        {
            PageSize = 10;
            PageIndex = 1;
        }
    }
}
