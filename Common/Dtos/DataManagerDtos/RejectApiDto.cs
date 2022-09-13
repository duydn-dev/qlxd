using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DataManagerDtos
{
    public class RejectApiDto
    {
        public int Id { get; set; }
        public int ApiNumber { get; set; }
        public string Message { get; set; }
    }
}
