﻿using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleCode { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? GroupRoleId { get; set; }

        public virtual GroupRole GroupRole { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}