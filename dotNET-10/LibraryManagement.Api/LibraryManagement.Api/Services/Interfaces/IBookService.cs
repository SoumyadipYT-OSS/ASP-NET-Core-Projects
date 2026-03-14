
using LibraryManagement.Api.DTOs;

namespace LibraryManagement.Api.Services.Interfaces;


public interface IBookService 
{
    Task<IEnumerable<BooksDto>> GetAllAsync();
    Task<BooksDto?> GetByIdAsync(int id);
    Task<int> AddAsync(CreateBookRequest request);
    Task<int> UpdateAsync(BooksDto bookDto);
    Task<int> DeleteAsync(int id);
}