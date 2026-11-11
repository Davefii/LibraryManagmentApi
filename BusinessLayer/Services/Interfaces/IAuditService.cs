using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IAuditService
    {
        Task<List<AuditLogResponseDTO>> LoadAuditLogsAsync();
        Task LogAsync(int? userId, string action, string entityName, string details);
    }

}
