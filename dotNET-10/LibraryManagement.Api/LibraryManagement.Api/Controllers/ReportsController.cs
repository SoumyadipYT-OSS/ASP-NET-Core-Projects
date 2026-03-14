using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagement.Api.Controllers; 


[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase 
{
    private readonly IReportsService _reportsService;

    public ReportsController(IReportsService reportsService) 
    {
        _reportsService = reportsService;
    }

    // GET: api/reports/overdue
    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<BorrowTransactions>>> GetOverdueBooks() 
    {
        var overdue = await _reportsService.GetOverdueBooksAsync();
        return Ok(overdue);
    }

    // GET: api/reports/member-stats/{memberId}
    [HttpGet("member-stats/{memberId}")]
    public async Task<ActionResult<MemberStatsDto>> GetMemberStats(int memberId) 
    {
        var stats = await _reportsService.GetMemberStatsAsync(memberId);
        if (stats == null)
            return NotFound(new { Message = "Member not found or no stats available" });

        return Ok(stats);
    }
}