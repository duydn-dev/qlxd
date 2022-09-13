using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.RoleDtos
{
    public class GroupRoleAndRoleDto
    {
        public Guid? GroupRoleId { get; set; }
        public string GroupRoleName { get; set; }
        public string GroupRoleCode { get; set; }

        public List<RoleDto> Roles { get; set; }
    }
}
