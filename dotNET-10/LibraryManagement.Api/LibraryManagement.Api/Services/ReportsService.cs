using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using LibraryManagement.Api.Services.Interfaces;

namespace LibraryManagement.Api.Services;

public class ReportsService : IReportsService 
{
    private readonly IReportsRepository _reportsRepository;

    public ReportsService(IReportsRepository reportsRepository) {
        _reportsRepository = reportsRepository;
    }

    public async Task<IEnumerable<BorrowTransactions>> GetOverdueBooksAsync() 
    {
        return await _reportsRepository.GetOverdueBooksAsync();
    }

    public async Task<MemberStatsDto?> GetMemberStatsAsync(int memberId) 
    {
        return await _reportsRepository.GetMemberStatsAsync(memberId);
    }
}