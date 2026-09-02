using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Repositories;

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

        public async Task<List<AuditLogResponseDTO>> LoadAuditLogsAsync()
        {
            var auditLogs = await _auditRepository.GetAllAsync();

            return auditLogs.Select(log => new AuditLogResponseDTO
            {
                Id = log.Id,
                UserId = log.UserId,
                Action = log.Action,
                EntityName = log.EntityName,
                Details = log.Details,
                CreatedAt = log.CreatedAt
            }).ToList();
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
