using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DepartmentDtos
{
    public class DepartmentGetPageDto
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string TextSearch { get; set; }

        public DepartmentGetPageDto()
        {
            PageSize = 10;
            PageIndex = 1;
        }
    }
}
