using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.RoleDtos
{
    public class GetRolesByUserDtos
    {
        public Guid UserId { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
    public class GetRolesByPositionDtos
    {
        public Guid PositionId { get; set; }
        public List<RoleDto> Roles { get; set; }
    }
}
