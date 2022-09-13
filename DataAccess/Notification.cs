using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class Notification
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsReaded { get; set; }
        public Guid? RecipientId { get; set; }
        public string Files { get; set; }
        public string Url { get; set; }
    }
}
