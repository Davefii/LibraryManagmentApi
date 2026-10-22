using BusinessLayer.DTOs;
using BusinessLayer.Services;
using BusinessLayer.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class AuditLogsController : ControllerBase
    {
        private readonly IAuditService _auditService;

        public AuditLogsController(IAuditService auditService)
        {
            _auditService = auditService;
        }

        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("LoadAuditLogs", Name = "LoadAuditLogs")]
        public async Task<IActionResult> LoadAuditLogs()
        {
            var auditLogs = await _auditService.LoadAuditLogsAsync();
            return Ok(auditLogs);
        }
    }
}
