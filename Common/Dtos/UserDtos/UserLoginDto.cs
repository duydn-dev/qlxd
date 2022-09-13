using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos
{
    public class UserLoginDto
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string GoogleAuthenCode { get; set; }

    }
    public class UserLoginResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string NumberPhone { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
        public string Avatar { get; set; }
        public bool? IsAdministrator { get; set; }   
        public int? LocalityId { get; set; }   
        public int? ParentLocalityId { get; set; }   
        public int? LocalityLevel { get; set; }
        public Guid? UserPositionId { get; set; }
        public bool IsBCT { get; set; }
    }
}
