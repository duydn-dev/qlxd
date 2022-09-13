using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class GroupRole
    {
        public GroupRole()
        {
            Roles = new HashSet<Role>();
        }

        public Guid GroupRoleId { get; set; }
        public string GroupRoleName { get; set; }
        public string GroupRoleCode { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}
