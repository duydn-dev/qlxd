using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Dtos.DocumentDtos
{
    public class DocumentViewDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string DocNumber { get; set; }
        public string Files { get; set; }
        public string IssuingUnit { get; set; }
        public DateTime? DateIssued { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
    }
}
