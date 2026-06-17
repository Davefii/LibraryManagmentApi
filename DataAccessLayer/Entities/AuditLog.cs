using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string Action { get; set; } = null!;

        public string EntityName { get; set; } = null!;

        public string Details { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
