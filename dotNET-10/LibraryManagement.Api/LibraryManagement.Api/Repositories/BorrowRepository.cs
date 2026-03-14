using Dapper;
using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using System.Data;

namespace LibraryManagement.Api.Repositories;


public class BorrowRepository : IBorrowRepository 
{
    private readonly IDbConnection _db;

    public BorrowRepository(IDbConnection db) 
    {
        _db = db;
    }

    public async Task<int> AddTransactionAsync(BorrowRequestDto request, IDbTransaction transaction) 
    {
        var sql = @"INSERT INTO BorrowTransactions (BookId, MemberId, BorrowDate, DueDate, FineAmount)
                VALUES (@BookId, @MemberId, GETDATE(), @DueDate, 0)";
        return await _db.ExecuteAsync(sql, request, transaction: transaction); 
    }


    public async Task<int> ReturnBookAsync(int transactionId, IDbTransaction transaction) 
    {
        var sql = "EXEC ReturnBook @TransactionId";
        return await _db.ExecuteAsync(
            sql,
            new { TransactionId = transactionId },
            transaction: transaction   
        );
    }


    public async Task<IEnumerable<BorrowTransactions>> GetAllTransactionsAsync() 
    {
        var sql = @"SELECT bt.TransactionId, bt.BookId, bt.MemberId, bt.BorrowDate, bt.DueDate, bt.ReturnDate, bt.FineAmount,
                       b.BookId, b.Title, b.Author, b.Category, b.CopiesAvailable,
                       m.MemberId, m.Name, m.Email, m.Phone, m.MembershipDate
                FROM BorrowTransactions bt
                INNER JOIN Books b ON bt.BookId = b.BookId
                INNER JOIN Members m ON bt.MemberId = m.MemberId";

        var result = await _db.QueryAsync<BorrowTransactions, Books, Members, BorrowTransactions>(
            sql,
            (bt, b, m) => {
                bt.Book = b;
                bt.Member = m;
                return bt;
            },
            splitOn: "BookId,MemberId"  // Dapper splitOn
        );

        return result;
    }


    public async Task<BorrowTransactions?> GetByIdAsync(int transactionId) 
    {
        var sql = @"SELECT bt.TransactionId, bt.BookId, bt.MemberId, bt.BorrowDate, bt.DueDate, bt.ReturnDate, bt.FineAmount,
                       b.BookId, b.Title, b.Author, b.Category, b.CopiesAvailable,
                       m.MemberId, m.Name, m.Email, m.Phone, m.MembershipDate
                FROM BorrowTransactions bt
                INNER JOIN Books b ON bt.BookId = b.BookId
                INNER JOIN Members m ON bt.MemberId = m.MemberId
                WHERE bt.TransactionId = @TransactionId";

        var result = await _db.QueryAsync<BorrowTransactions, Books, Members, BorrowTransactions>(
            sql,
            (bt, b, m) => {
                bt.Book = b;     // attach full book object
                bt.Member = m;   // attach full member object
                return bt;
            },
            new { TransactionId = transactionId },
            splitOn: "BookId,MemberId"   // ← Dapper multiple split on
        );

        return result.FirstOrDefault();
    }


    public async Task<BorrowTransactions?> GetByIdAsync(int transactionId, IDbTransaction transaction) 
    {
        var sql = @"SELECT bt.TransactionId, bt.BookId, bt.MemberId, bt.BorrowDate, bt.DueDate, bt.ReturnDate, bt.FineAmount,
                       b.BookId, b.Title, b.Author, b.Category, b.CopiesAvailable,
                       m.MemberId, m.Name, m.Email, m.Phone, m.MembershipDate
                FROM BorrowTransactions bt
                INNER JOIN Books b ON bt.BookId = b.BookId
                INNER JOIN Members m ON bt.MemberId = m.MemberId
                WHERE bt.TransactionId = @TransactionId";

        var result = await _db.QueryAsync<BorrowTransactions, Books, Members, BorrowTransactions>(
            sql,
            (bt, b, m) => { bt.Book = b; bt.Member = m; return bt; },
            new { TransactionId = transactionId },
            transaction: transaction,   
            splitOn: "BookId,MemberId"
        );

        return result.FirstOrDefault();
    }

}
