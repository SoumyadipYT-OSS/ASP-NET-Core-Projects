
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Repositories.Interfaces;


public interface IMemberRepository 
{
    Task<IEnumerable<Members>> GetAllAsync();
    Task<Members?> GetByIdAsync(int id);
    Task<int> AddAsync(Members member);
    Task<int> UpdateAsync(Members member);
    Task<int> DeleteAsync(int id);


    // Borrowing history for a member 
    Task<IEnumerable<MemberHistoryDto>> GetBorrowHistoryAsync(int memberId); 
}
