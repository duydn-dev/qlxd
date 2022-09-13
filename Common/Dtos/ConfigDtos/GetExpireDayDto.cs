using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.ConfigDtos
{
    public class GetExpireDayDto
    {
        public DateTime? ToDay { get; set; }
        public DateTime? ThisWeekStart { get; set; }
        public DateTime? ThisWeekEnd { get; set; }
        public DateTime? ThisExpireDate { get; set; }
        public DateTime? PrevWeekStart { get; set; }
        public DateTime? PrevWeekEnd { get; set; }
        public DateTime? ThisMonthStart { get; set; }
        public DateTime? ThisMonthEnd { get; set; }
        public DateTime? PrevMonthStart { get; set; }
        public DateTime? PrevMonthEnd { get; set; }
    }
}
