using DataAccessLayer.Repositories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IDashboardService
    {
        Task<DashboardDTO> Dashboard();
        Task<List<PopularBookDTO>> GetPopularBooksAsync();
        Task<List<PopularBookDTO>> GetPopularBooksReturnedAsync();
        Task<List<BooksbycategoryDTO>> GetBooksByCategoryAsync();
        Task<List<RecentborrowingsDTO>> GetRecentborrowingsAsync();
        Task<int> TotalBorrowingsForSelfMemberAsync(int MemberId);
        Task<int> TotalActiveBorrowingsForSelfMemberAsync(int MemberId);
    }
}
