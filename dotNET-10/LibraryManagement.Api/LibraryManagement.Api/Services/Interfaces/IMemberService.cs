using LibraryManagement.Api.DTOs;

namespace LibraryManagement.Api.Services.Interfaces;


public interface IMemberService 
{
    Task<IEnumerable<MembersDto>> GetAllAsync();
    Task<MembersDto?> GetByIdAsync(int id);
    Task<int> RegisterAsync(CreateMemberRequest request);
    Task<int> UpdateAsync(MembersDto memberDto);
    Task<int> DeleteAsync(int id);

    // Borrowing history for a member
    Task<IEnumerable<MemberHistoryDto>> GetBorrowHistoryAsync(int memberId);
}