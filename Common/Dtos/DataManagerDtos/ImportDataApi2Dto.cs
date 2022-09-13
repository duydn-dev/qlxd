using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class ImportDataApi2Dto
    {
        public int? MaDoiTuongCha { get; set; }
        public long? Customer { get; set; }
        public string TenNhaPP { get; set; }
        public string SDT { get; set; }
        public string DiaChi { get; set; }
        public int LoaiDoiTuongCha { get; set; }
        public int LoaiDoiTuongCon { get; set; }

    }
}
