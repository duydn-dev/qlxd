using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.TongNguonPhanGiaoDtos
{
    public class TongNguongPhanGiaoPageDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TextSearch { get; set; }
        public int? Nam { get; set; }
        public TongNguongPhanGiaoPageDto()
        {
            PageSize = 20;
            PageIndex = 1;
        }
    }
}
