using LibraryManagement.Api.DTOs;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using LibraryManagement.Api.Services.Interfaces;

namespace LibraryManagement.Api.Services;


public class BookService : IBookService 
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository) 
    {
        _bookRepository = bookRepository;
    }


    public async Task<IEnumerable<BooksDto>> GetAllAsync() 
    {
        var books = await _bookRepository.GetAllAsync();
        return books.Select(b => new BooksDto {
            BookId = b.BookId,
            Title = b.Title,
            Author = b.Author,
            Category = b.Category,
            CopiesAvailable = b.CopiesAvailable
        });
    }

    public async Task<BooksDto?> GetByIdAsync(int id) 
    {
        var book = await _bookRepository.GetByIdAsync(id);
        if (book == null) return null;

        return new BooksDto {
            BookId = book.BookId,
            Title = book.Title,
            Author = book.Author,
            Category = book.Category,
            CopiesAvailable = book.CopiesAvailable
        };
    }

    public async Task<int> AddAsync(CreateBookRequest request) 
    {
        var book = new Books {
            Title = request.Title,
            Author = request.Author,
            ISBN = request.ISBN,
            PublishedYear = request.PublishedYear,
            Category = request.Category,
            CopiesAvailable = request.CopiesAvailable
        };

        return await _bookRepository.AddAsync(book);
    }

    public async Task<int> UpdateAsync(BooksDto bookDto) 
    {
        var book = new Books 
        {
            BookId = bookDto.BookId,
            Title = bookDto.Title,
            Author = bookDto.Author,
            Category = bookDto.Category,
            CopiesAvailable = bookDto.CopiesAvailable
        };

        return await _bookRepository.UpdateAsync(book);
    }

    public async Task<int> DeleteAsync(int id) 
    {
        return await _bookRepository.DeleteAsync(id);
    }
}
