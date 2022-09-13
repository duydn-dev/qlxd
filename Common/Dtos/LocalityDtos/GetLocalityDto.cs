using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.LocalityDtos
{
    public class GetLocalityDto
    {
        public int? Type { get; set; }
        public int? ParentId { get; set; }
    }
}
