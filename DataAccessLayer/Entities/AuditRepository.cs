using DataAccessLayer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Entities
{
    public class AuditRepository
    {
        private readonly AppDbContext _context;

        public AuditRepository(
            AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            AuditLog auditLog)
        {
            await _context.AuditLogs
                .AddAsync(auditLog);

            await _context.SaveChangesAsync();
        }
    }
}
