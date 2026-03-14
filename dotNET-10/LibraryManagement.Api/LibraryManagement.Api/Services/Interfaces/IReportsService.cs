using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Services.Interfaces;

public interface IReportsService 
{
    Task<IEnumerable<BorrowTransactions>> GetOverdueBooksAsync();
    Task<MemberStatsDto?> GetMemberStatsAsync(int memberId);
}