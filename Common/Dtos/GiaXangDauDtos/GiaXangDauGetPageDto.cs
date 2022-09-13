using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.GiaXangDauDtos
{
    public class GiaXangDauGetPageDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TextSearch { get; set; }
        public int? Vung { get; set; }
        public GiaXangDauGetPageDto()
        {
            PageSize = 20;
            PageIndex = 1;
        }
    }
}
