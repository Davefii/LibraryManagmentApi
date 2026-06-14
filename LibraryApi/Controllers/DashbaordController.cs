using BusinessLayer.DTOs;
using BusinessLayer.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryApi.Controllers
{
    [Authorize]
    public class DashbaordController : Controller
    {
        private readonly DashbaordService _dashboardService;

        public DashbaordController(
            DashbaordService dashboardService)
        {
            _dashboardService = dashboardService;
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("Dashboard", Name = "Dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var dashboard =
                await _dashboardService.Dashboard();

            return Ok(dashboard);
        }
    }
}
