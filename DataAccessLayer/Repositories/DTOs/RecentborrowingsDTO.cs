using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.DTOs
{
    public class RecentborrowingsDTO
    {
        public int BorrowingId { get; set; }
        public string MemberName { get; set; }
        public string BookTitle { get; set; }
        public string Status { get; set; }
        public DateTime Datee { get; set; }
    }
}
