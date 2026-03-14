using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Repositories.Interfaces;

public interface IReportsRepository 
{
    Task<IEnumerable<BorrowTransactions>> GetOverdueBooksAsync();
    Task<MemberStatsDto?> GetMemberStatsAsync(int memberId);
}