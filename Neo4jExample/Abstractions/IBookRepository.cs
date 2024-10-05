using Neo4jExample.Models;

namespace Neo4jExample.Abstractions;

public interface IBookRepository
{
    Task<Book?> GetBookByTitleAsync(string title);
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<bool> CreateBookAsync(Book book);
    Task<bool> UpdateBookAsync(string title, Book book);
    Task<bool> DeleteBookAsync(string title);
}
