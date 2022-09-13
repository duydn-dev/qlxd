﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.ActionLogs
{
    public class CreateActionLogDto
    {
        public long Id { get; set; }
        public Guid? UserId { get; set; }
        public string ContentLog { get; set; }
        public string Url { get; set; }
        public DateTime TimeLine { get; set; }
    }
}
