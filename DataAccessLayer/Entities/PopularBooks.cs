using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class PopularBooks
    {
        public int BookId { get; set; }

        public string Title { get; set; } = null!;

        public int BorrowCount { get; set; }
    }
}
