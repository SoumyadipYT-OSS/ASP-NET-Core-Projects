using Dapper;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using System.Data;

namespace LibraryManagement.Api.Repositories;


public class ReportsRepository : IReportsRepository 
{
    private readonly IDbConnection _db;

    public ReportsRepository(IDbConnection db) 
    {
        _db = db;
    }

    public async Task<IEnumerable<BorrowTransactions>> GetOverdueBooksAsync() 
    {
        var sql = @"SELECT bt.TransactionId, bt.BookId, bt.MemberId, bt.BorrowDate, bt.DueDate, bt.ReturnDate, bt.FineAmount,
                           b.BookId, b.Title, b.Author, b.Category, b.CopiesAvailable,
                           m.MemberId, m.Name, m.Email, m.Phone, m.MembershipDate
                    FROM BorrowTransactions bt
                    INNER JOIN Books b ON bt.BookId = b.BookId
                    INNER JOIN Members m ON bt.MemberId = m.MemberId
                    WHERE bt.ReturnDate IS NULL AND bt.DueDate < GETDATE()";

        var result = await _db.QueryAsync<BorrowTransactions, Books, Members, BorrowTransactions>(
            sql,
            (bt, b, m) => { bt.Book = b; bt.Member = m; return bt; },
            splitOn: "BookId,MemberId"
        );

        return result;
    }

    public async Task<MemberStatsDto?> GetMemberStatsAsync(int memberId) 
    {
        var sql = @"SELECT 
                        m.MemberId,
                        m.Name,
                        COUNT(bt.TransactionId) AS TotalBorrowed,
                        SUM(CASE WHEN bt.ReturnDate IS NULL THEN 1 ELSE 0 END) AS CurrentlyBorrowed,
                        SUM(CASE WHEN bt.ReturnDate IS NULL AND bt.DueDate < GETDATE() THEN 1 ELSE 0 END) AS OverdueCount
                    FROM Members m
                    LEFT JOIN BorrowTransactions bt ON m.MemberId = bt.MemberId
                    WHERE m.MemberId = @MemberId
                    GROUP BY m.MemberId, m.Name";

        return await _db.QueryFirstOrDefaultAsync<MemberStatsDto>(
            sql,
            new { MemberId = memberId }
        );
    }
}
