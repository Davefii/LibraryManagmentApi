using BusinessLayer.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Interface
{
    public interface IBorrowingService
    {
        Task<List<OverdueBorrowingDTO>> GetOverdueBorrowings();
        Task<List<BorrowingResponseDTO>> GetAllBorrowings();
        Task<List<BorrowingResponseDTO>> GetAllBorrowingsByMemberName(string memberName);
        Task<List<BorrowingResponseDTO>> GetAllBorrowingsByUserEmail(string email);
        Task<List<BorrowingResponseDTO>> GetAllBorrowingsByBookTitle(string bookTitle);
        Task<List<BorrowingResponseDTO>> GetAllBorrowingsReturnedByStatus(string status);
        Task<BorrowingResponseDTO?> GetBorrowingById(int id);
        Task<List<BorrowingResponseDTO>> GetMemberBorrowings(int memberId);
        Task AddBorrowing(CreateBorrowingDTO dto);
        Task UpdateBorrowing(int id, UpdateBorrowingDTO dto);
        Task ReturnBook(int borrowingId);
        Task DeleteBorrowing(int id);
        Task<List<RecentborrowingsDTO>> RecentborrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalBorrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalActiveBorrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalReturnBorrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalOverdueBorrowingsForSelfMemberAsync(int memberId);
        Task<int?> GetMemberIdByUserIdAsync(int userId);
    }
}
