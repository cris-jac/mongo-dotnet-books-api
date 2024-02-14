using API.Models;
using MongoDB.Bson;

namespace API.Interfaces;

public interface IReaderRepository
{
    Task<IEnumerable<Reader>> GetAllAsync();
    Task<Reader> GetReaderById(string id);
    Task<Reader> GetReaderByUsername(string username);
    Task<bool> ReaderExists(string id);
    Task AddReader(Reader reader);
    // Task<IEnumerable<ObjectId>> GetBooksFromReader(string readerId);
    Task<bool> AddBookToList(string readerId, string bookId);
    Task<bool> RemoveBookFromList(string readerId, string bookId);
    Task<bool> UpdateReaderNationality(string readerId, string nationalityId);
}