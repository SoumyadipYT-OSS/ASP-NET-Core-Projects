
using LibraryManagement.Api.Models;
using System.Data;

namespace LibraryManagement.Api.Repositories.Interfaces;

public interface IBookRepository 
{
    Task<IEnumerable<Books>> GetAllAsync();
    Task<Books> GetByIdAsync(int id);
    Task<Books?> GetByIdAsync(int id, IDbTransaction? transaction = null); 
    Task<int> AddAsync(Books book);
    Task<int> UpdateAsync(Books book);
    Task<int> UpdateAsync(Books book, IDbTransaction? transaction = null);
    Task<int> DeleteAsync(int id);
    Task<IEnumerable<Books>> SearchAsync(string title, string author, string category); 
}
