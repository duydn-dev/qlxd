using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.RoleDtos
{
    public class RoleDto
    {
        public Guid? RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? GroupRoleId { get; set; }
    }
}
