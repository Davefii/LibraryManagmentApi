using DataAccessLayer.Repositories.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<PopularBookDTO>> GetPopularBooksAsync();
        Task<List<PopularBookDTO>> GetPopularBooksReturnedAsync();
        Task<List<BooksbycategoryDTO>> GetBooksByCategoryAsync();
    }
}
