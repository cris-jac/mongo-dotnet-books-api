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
    // Task<IEnumerable<Book>> GetBooksByAuthor(string authorId);
    // Task<IEnumerable<Book>> GetBooksByCategory(string categoryId);
    // Task<IEnumerable<Book>> GetBooksByPublisher(string publisherId);

    // Get 
    Task<IEnumerable<Book>> GetBooksByAuthor(string authorId);
    Task<IEnumerable<Book>> GetBooksByCategory(string categoryId);
    Task<IEnumerable<Book>> GetBooksByPublisher(string publisherId);

    // Updates
    Task<bool> AddAuthorToBook(string bookId, string authorId);
    Task<bool> RemoveAuthorFromBook(string bookId, string authorId);
    Task<bool> AddCategoryToBook(string bookId, string categoryId);
    Task<bool> RemoveCategoryFromBook(string bookId, string categoryId);
    Task<bool> AddPublisherToBook(string bookId, string publisherId);
    Task<bool> RemovePublisherFromBook(string bookId, string publisherId);

    //
    Task<List<string>> GetBooksByIds(List<string> booksIds);
} 