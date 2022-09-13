using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DocumentDtos
{
    public class DocumentGetListRequestDto : BaseSearchDto
    {
        public int? GroupId { get; set; }
    }
}
