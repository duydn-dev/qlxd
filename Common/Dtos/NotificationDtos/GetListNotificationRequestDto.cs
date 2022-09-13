using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.NotificationDtos
{
    public class GetListNotificationRequestDto
    {
        public Guid UserId { get; set; }
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public GetListNotificationRequestDto()
        {
            PageSize = 10;
            PageIndex = 1;
        }
    }
}
