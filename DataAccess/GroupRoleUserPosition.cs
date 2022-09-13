using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class GroupRoleUserPosition
    {
        public Guid GroupRoleUserPositionId { get; set; }
        public Guid? PositionUserId { get; set; }
        public Guid? RoleId { get; set; }
    }
}
