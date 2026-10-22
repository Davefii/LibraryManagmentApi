using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.DTOs
{
    public class DashboardDTO
    {
        public int TotalBooks { get; set; }
        public int TotalAuthors { get; set; }
        public int TotalCategories { get; set; }
        public int TotalMembers { get; set; }
        public int ActiveBorrowings { get; set; }
        public int OverdueBorrowings { get; set; }
        public int BooksUnavailable { get; set; }
    }
}
