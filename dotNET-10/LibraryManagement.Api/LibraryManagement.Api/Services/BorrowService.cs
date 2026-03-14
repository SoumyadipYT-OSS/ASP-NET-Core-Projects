using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using LibraryManagement.Api.Services.Interfaces;
using System.Data;

namespace LibraryManagement.Api.Services;


public class BorrowService : IBorrowService 
{
    private readonly IDbConnection _db;
    private readonly IBookRepository _bookRepository;
    private readonly IBorrowRepository _borrowRepository;

    public BorrowService(IDbConnection db, IBookRepository bookRepository, IBorrowRepository borrowRepository) 
    {
        _db = db;
        _bookRepository = bookRepository;
        _borrowRepository = borrowRepository;
    }


    public async Task<BorrowResponseDto> BorrowBookAsync(BorrowRequestDto request) 
    {
        if (_db.State != ConnectionState.Open)
            _db.Open();

        using var transaction = _db.BeginTransaction();

        try 
        {
            var book = await _bookRepository.GetByIdAsync(request.BookId, transaction);
            if (book == null)
                return new BorrowResponseDto { Success = false, Message = "Book not found." };

            if (book.CopiesAvailable <= 0)
                return new BorrowResponseDto { Success = false, Message = "No copies available." };

            await _borrowRepository.AddTransactionAsync(request, transaction);

            book.CopiesAvailable -= 1;
            await _bookRepository.UpdateAsync(book, transaction);

            transaction.Commit();
            return new BorrowResponseDto { Success = true, Message = "Book borrowed successfully." };
        } catch (Exception ex) {
            transaction.Rollback();
            return new BorrowResponseDto { Success = false, Message = $"Error borrowing book: {ex.Message}" };
        }
    }

    public async Task<BorrowResponseDto> ReturnBookAsync(int transactionId) 
    {
        if (_db.State != ConnectionState.Open)
            _db.Open();

        using var transaction = _db.BeginTransaction();

        try 
        {
            // Update return date and fine calculation
            await _borrowRepository.ReturnBookAsync(transactionId, transaction);

            // Fetch transaction to get BookId (inside transaction)
            var borrowTransaction = await _borrowRepository.GetByIdAsync(transactionId, transaction);

            if (borrowTransaction == null)
                return new BorrowResponseDto { Success = false, Message = "Transaction not found." };

            // Increment available copies (inside transaction)
            var book = await _bookRepository.GetByIdAsync(borrowTransaction.BookId, transaction); // you can add overload if needed
            if (book != null) {
                book.CopiesAvailable += 1;
                await _bookRepository.UpdateAsync(book, transaction);
            }

            transaction.Commit();
            return new BorrowResponseDto { Success = true, Message = "Book returned successfully." };
        } catch (Exception ex) {
            transaction.Rollback();
            return new BorrowResponseDto { Success = false, Message = $"Error returning book: {ex.Message}" };
        }
    }

    public async Task<IEnumerable<BorrowTransactions>> GetAllTransactionsAsync() 
    {
        return await _borrowRepository.GetAllTransactionsAsync();
    }

    public async Task<BorrowTransactions?> GetTransactionByIdAsync(int transactionId) 
    {
        return await _borrowRepository.GetByIdAsync(transactionId);
    }
}
