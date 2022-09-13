using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.UserPositionDtos
{
    public class UserPositionDto
    {
        public Guid? UserPositionId { get; set; }
        public string UserPositionName { get; set; }
        public bool? IsAdministrator { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
        public string LocalityAccept { get; set; }
    }
}
