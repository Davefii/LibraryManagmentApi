
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interfaces
{
    public interface IBorrowingRepository
    {
        Task<List<Borrowing>> GetAllAsync();
        Task<List<Borrowing>> GetAllBorrowingsByMemberNameAsync(string memberName);
        Task<List<Borrowing>> GetAllBorrowingsByUserNameAsync(string userEmail);
        Task<List<Borrowing>> GetAllBorrowingsReturnedByStatusAsync(string status);
        Task<List<Borrowing>> GetAllBorrowingsByBookTitleAsync(string bookTitle);
        Task<Borrowing?> GetByIdAsync(int id);
        Task AddAsync(Borrowing borrowing);
        Task UpdateAsync(Borrowing borrowing);
        Task DeleteAsync(Borrowing borrowing);
        Task<Borrowing?> GetActiveBorrowingAsync(int memberId, int bookId);
        Task<int> CountActiveBorrowingsAsync(int memberId);
        Task<List<Borrowing>> GetOverdueBorrowingsAsync();
        Task<int> TotalBorrowings();
        Task<int> GetOverdueBorrowingsCountAsync();
        Task<List<Borrowing>> GetBorrowingsByMemberIdsAsync(List<int> memberIds);
        Task<List<Borrowing>> GetBorrowingsByMemberIdAsync(int memberId);
        Task<List<Borrowing>> GetPopularBooksAsync();
        Task<List<Borrowing>> GetPopularBooksReturned();
        Task<List<Borrowing>> Recentborrowings();
        Task<List<Borrowing>> RecentborrowingsForSelfMember(int memberId);
        Task<int> TotalBorrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalActiveBorrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalReturnBorrowingsForSelfMemberAsync(int memberId);
        Task<int> TotalOverduebooksBorrowingsForSelfMemberAsync(int memberId);
    }
}
