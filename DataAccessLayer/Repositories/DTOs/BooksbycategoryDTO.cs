using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.DTOs
{
    public class BooksbycategoryDTO
    {
        public string NameCategory { get; set; } = null!;
        public int TotalBooks { get; set; }
    }
}
