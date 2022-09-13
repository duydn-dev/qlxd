using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class UserPosition
    {
        public UserPosition()
        {
            Users = new HashSet<User>();
        }

        public Guid UserPositionId { get; set; }
        public string UserPositionName { get; set; }
        public bool? IsAdministrator { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Status { get; set; }
        public string LocalityAccept { get; set; }

        public virtual ICollection<User> Users { get; set; }
    }
}
