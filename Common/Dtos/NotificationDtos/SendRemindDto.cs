using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.NotificationDtos
{
    public class SendRemindDto
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public bool? IsReaded { get; set; }
        public Guid? RecipientId { get; set; }
        public int LocalityId { get; set; } 
        public string Files { get; set; } 
        public int MaDoiTuong { get; set; }
    }
}
