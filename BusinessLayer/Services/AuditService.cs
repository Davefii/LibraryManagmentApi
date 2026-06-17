using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class AuditService
    {
        private readonly AuditRepository _auditRepository;

        public AuditService(
            AuditRepository auditRepository)
        {
            _auditRepository = auditRepository;
        }

        public async Task LogAsync(
            int? userId,
            string action,
            string entityName,
            string details)
        {
            var auditLog = new AuditLog
            {
                UserId = userId,
                Action = action,
                EntityName = entityName,
                Details = details,
                CreatedAt = DateTime.UtcNow
            };

            await _auditRepository
                .AddAsync(auditLog);
        }
    }
}
