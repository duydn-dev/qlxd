using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class BaseSearchDto
    {
        public string TextSearch { get; set; }
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }
        public BaseSearchDto()
        {
            PageIndex = 1;
            PageSize = 20;
        }
    }

}
