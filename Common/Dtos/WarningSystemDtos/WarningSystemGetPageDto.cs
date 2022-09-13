using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.WarningSystemDtos
{
    public class WarningSystemGetPageDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TextSearch { get; set; }
        public WarningSystemGetPageDto()
        {
            PageSize = 100;
            PageIndex = 1;
        }
    }
}
