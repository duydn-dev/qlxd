using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.WarningSystemDtos
{
    public class CreateWarningSystemDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public Guid UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
