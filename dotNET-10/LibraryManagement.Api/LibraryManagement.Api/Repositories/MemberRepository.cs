using Dapper;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using System.Data; 

namespace LibraryManagement.Api.Repositories;


public class MemberRepository : IMemberRepository 
{
    private readonly IDbConnection _db;

    public MemberRepository(IDbConnection db) 
    {
        _db = db;
    }

    public async Task<IEnumerable<Members>> GetAllAsync() 
    {
        var sql = "SELECT * FROM Members";
        return await _db.QueryAsync<Members>(sql);
    }

    public async Task<Members?> GetByIdAsync(int id) 
    {
        var sql = "SELECT * FROM Members WHERE MemberId = @Id";
        return await _db.QueryFirstOrDefaultAsync<Members>(sql, new { Id = id });
    }

    public async Task<int> AddAsync(Members member) 
    {
        var sql = @"INSERT INTO Members (Name, Email, Phone, MembershipDate)
                    VALUES (@Name, @Email, @Phone, @MembershipDate)";
        return await _db.ExecuteAsync(sql, member);
    }

    public async Task<int> UpdateAsync(Members member) 
    {
        var sql = @"UPDATE Members 
                    SET Name=@Name, Email=@Email, Phone=@Phone
                    WHERE MemberId=@MemberId";
        return await _db.ExecuteAsync(sql, member);
    }

    public async Task<int> DeleteAsync(int id) 
    {
        var sql = "DELETE FROM Members WHERE MemberId=@Id";
        return await _db.ExecuteAsync(sql, new { Id = id });
    }


    // Borrowing history for a member 
    public async Task<IEnumerable<MemberHistoryDto>> GetBorrowHistoryAsync(int memberId) 
    {
        var sql = @"SELECT bt.TransactionId, b.Title AS BookTitle, 
                           bt.BorrowDate, bt.ReturnDate, bt.FineAmount
                    FROM BorrowTransactions bt
                    INNER JOIN Books b ON bt.BookId = b.BookId
                    WHERE bt.MemberId = @MemberId";

        return await _db.QueryAsync<MemberHistoryDto>(sql, new { MemberId = memberId });
    }
}
