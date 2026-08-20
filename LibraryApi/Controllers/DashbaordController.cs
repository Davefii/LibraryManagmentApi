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
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("PopularBooks")]
        public async Task<IActionResult> PopularBooks()
        {
            var books =
                await _dashboardService
                    .GetPopularBooks();

            return Ok(books);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("PopularBooksIsReturned")]
        public async Task<IActionResult> PopularBooksIsReturned()
        {
            var Popularbooks =
                await _dashboardService
                    .GetPopularBooksReturned();

            return Ok(Popularbooks);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("BooksByCategory")]
        public async Task<IActionResult> BooksByCategory()
        {
            var BooksbyCategory = await _dashboardService.GetBooksByCategory();
            return Ok(BooksbyCategory);
        }
        [Authorize(Roles = $"{Roles.Admin}")]
        [HttpGet("Recentborrowings")]
        public async Task<IActionResult> RecentBorrowings()
        {
            var recentBorrowings = await _dashboardService.GetRecentborrowingsAsync();
            return Ok(recentBorrowings);
        }
    }
}
