using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.UserDtos
{
    public class ListUserResponseDto
    {
        public Guid? UserId { get; set; }
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
        public string UserPositionName { get; set; }
        public bool? IsAdministrator { get; set; }
        public int? LocalityId { get; set; }
        public int? LocalityLevel { get; set; }
        public int? Province { get; set; }
        public int? District { get; set; }
        public int? Ward { get; set; }
        public string CreatedCode { get; set; }
    }
}
