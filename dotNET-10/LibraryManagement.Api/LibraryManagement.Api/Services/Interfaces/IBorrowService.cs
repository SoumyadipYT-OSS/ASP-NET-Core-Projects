using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;

namespace LibraryManagement.Api.Services.Interfaces;

public interface IBorrowService 
{
    Task<BorrowResponseDto> BorrowBookAsync(BorrowRequestDto request);
    Task<BorrowResponseDto> ReturnBookAsync(int transactionId);
    Task<IEnumerable<BorrowTransactions>> GetAllTransactionsAsync();
    Task<BorrowTransactions?> GetTransactionByIdAsync(int transactionId);
}