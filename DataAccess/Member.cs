using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class Member
    {
        public string Acc { get; set; }
        public string Pass { get; set; }
        public string PassReset { get; set; }
        public string Email { get; set; }
        public string Tel { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
        public string InfomationOther { get; set; }
        public int? Status { get; set; }
        public int? Lever { get; set; }
        public int? Point { get; set; }
        public string ListAccess { get; set; }
        public int? CompanyId { get; set; }
    }
}
