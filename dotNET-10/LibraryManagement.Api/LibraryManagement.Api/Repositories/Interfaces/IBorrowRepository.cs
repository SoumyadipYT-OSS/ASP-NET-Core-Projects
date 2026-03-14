using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using System.Data;

namespace LibraryManagement.Api.Repositories.Interfaces;


public interface IBorrowRepository 
{
    Task<int> AddTransactionAsync(BorrowRequestDto request, IDbTransaction transaction);
    Task<int> ReturnBookAsync(int transactionId, IDbTransaction transaction);
    Task<IEnumerable<BorrowTransactions>> GetAllTransactionsAsync();
    Task<BorrowTransactions?> GetByIdAsync(int transactionId);
    Task<BorrowTransactions?> GetByIdAsync(int transactionId, IDbTransaction transaction);
}

