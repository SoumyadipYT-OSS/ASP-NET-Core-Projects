using Dapper;
using LibraryManagement.Api.Models;
using LibraryManagement.Api.Repositories.Interfaces;
using System.Data;

namespace LibraryManagement.Api.Repositories;


public class BookRepository : IBookRepository
{
    private readonly IDbConnection _db; 

    public BookRepository(IDbConnection db) 
    {
        _db = db;
    }


    public async Task<IEnumerable<Books>> GetAllAsync() 
    {
        var sql = "SELECT * FROM dbo.Books";
        return await _db.QueryAsync<Books>(sql);
    }

    public async Task<Books> GetByIdAsync(int id) 
    {
        var sql = "SELECT * FROM dbo.Books WHERE BookId = @Id";
        return (await _db.QueryFirstOrDefaultAsync<Books>(sql, new { Id = id }))!;
    }

    public async Task<Books?> GetByIdAsync(int bookId, IDbTransaction? transaction = null) 
    {
        var sql = @"SELECT BookId, Title, Author, Category, CopiesAvailable
                FROM Books
                WHERE BookId = @BookId";

        return await _db.QueryFirstOrDefaultAsync<Books>(
            sql,
            new { BookId = bookId },
            transaction: transaction   
        );
    }


    public async Task<int> AddAsync(Books book) 
    {
        var sql = @"INSERT INTO Books (Title, Author, ISBN, PublishedYear, Category, CopiesAvailable)
                    VALUES (@Title, @Author, @ISBN, @PublishedYear, @Category, @CopiesAvailable)";
        return await _db.ExecuteAsync(sql, book);
    }

    public async Task<int> UpdateAsync(Books book) 
    {
        var sql = @"UPDATE Books 
                    SET Title=@Title, Author=@Author, ISBN=@ISBN, PublishedYear=@PublishedYear, 
                        Category=@Category, CopiesAvailable=@CopiesAvailable
                    WHERE BookId=@BookId";
        return await _db.ExecuteAsync(sql, book);
    }

    public async Task<int> UpdateAsync(Books book, IDbTransaction? transaction = null) 
    {
        var sql = @"UPDATE Books 
                SET Title=@Title, Author=@Author, Category=@Category, CopiesAvailable=@CopiesAvailable
                WHERE BookId=@BookId";
        return await _db.ExecuteAsync(sql, book, transaction: transaction);
    }


    public async Task<int> DeleteAsync(int id) 
    {
        var sql = "DELETE FROM Books WHERE BookId=@Id";
        return await _db.ExecuteAsync(sql, new { Id = id });
    }

    public async Task<IEnumerable<Books>> SearchAsync(string title, string author, string category) 
    {
        var sql = @"SELECT * FROM Books 
                    WHERE (@Title IS NULL OR Title LIKE '%' + @Title + '%')
                      AND (@Author IS NULL OR Author LIKE '%' + @Author + '%')
                      AND (@Category IS NULL OR Category LIKE '%' + @Category + '%')";
        return await _db.QueryAsync<Books>(sql, new { Title = title, Author = author, Category = category });
    }
}
