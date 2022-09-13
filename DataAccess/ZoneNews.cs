using System;
using System.Collections.Generic;

#nullable disable

namespace DataAccess
{
    public partial class ZoneNews
    {
        public int ZoneId { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public int? ParentId { get; set; }
        public int? Status { get; set; }
    }
}
