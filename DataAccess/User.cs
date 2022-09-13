using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class User
    {
        public User()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Avatar { get; set; }
        public string Address { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
        public Guid? UserPositionId { get; set; }
        public Guid? DepartmentId { get; set; }
        public int? LocalityId { get; set; }
        public int? LocalityLevel { get; set; }
        public int? Province { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public bool IsBCT { get; set; }
        public string CreatedCode { get; set; }
        public int? MaDoiTuong { get; set; }

        public virtual Department Department { get; set; }
        public virtual UserPosition UserPosition { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
