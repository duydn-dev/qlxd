using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class StoreHouseCategory
    {
        public long Id { get; set; }
        public string NumNo { get; set; }
        public string Name { get; set; }    
        public int? Priority { get; set; }
        public int? LocalityId { get; set; }
        public int? ParentId { get; set; }
    }
}
