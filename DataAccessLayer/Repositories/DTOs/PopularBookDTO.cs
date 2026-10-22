using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.DTOs
{
    public class PopularBookDTO
    {
        public int BookId { get; set; }
        public string Title { get; set; } = null!;
        public int BorrowCount { get; set; }
    }
}
