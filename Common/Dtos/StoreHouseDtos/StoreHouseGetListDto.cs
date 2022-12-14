using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.StoreHouseDtos
{
    public class StoreHouseGetListDto
    {
        public int? Index { get; set; }
        public long? Id { get; set; }
        public int Total { get; set; }
        public int? Priority { get; set; }
        public string StoreName { get; set; }
        public string Address { get; set; }
        public string UnitManager { get; set; }
        public float? Wattage { get; set; }
        public float? Capacity { get; set; }
        public string Dwt { get; set; }
        public string Nature { get; set; }
        public string ZoneOfInfluence { get; set; }
        public long? StoreHouseCategoryId { get; set; }
        public bool IsCate { get; set; }
    }
    public class StoreHouseGetListRequestDto : BaseSearchDto
    {
    }
}
