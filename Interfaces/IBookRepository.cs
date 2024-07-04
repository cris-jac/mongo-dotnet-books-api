using API.Models;

namespace API.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllBooks();
    Task<Book> GetBookById(string id);
    Task AddAsync(Book book);
    Task UpdateAsync(string id, Book book);
    Task<bool> RemoveAsync(string id);
    Task<bool> BookExists(string id);

    // Get 
    Task<IEnumerable<Book>> GetBooksByAuthor(string authorId);
    Task<IEnumerable<Book>> GetBooksByCategory(string categoryId);
    Task<IEnumerable<Book>> GetBooksByPublisher(string publisherId);

    // Updates
    Task<bool> AddRemoveAuthorToBook(string bookId, string authorId);
    Task<bool> AddRemoveCategoryToBook(string bookId, string categoryId);
    Task<bool> AddRemovePublisherToBook(string bookId, string publisherId);

    //
    Task<List<string>> GetBooksByIds(List<string> booksIds);
}