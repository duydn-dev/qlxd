using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public  class WarningSystem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public int Type { get; set; }
        public Guid UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
