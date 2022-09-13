using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class UsersReference
    {
        public long Id { get; set; }
        public int? MaDoiTuong { get; set; }
        public string EncryptUser { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int ApiType { get; set; }
    }
}
