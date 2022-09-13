using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class Department
    {
        public Department()
        {
            Users = new HashSet<User>();
        }

        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
