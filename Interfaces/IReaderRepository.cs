using API.Models;
using MongoDB.Bson;

namespace API.Interfaces;

public interface IReaderRepository
{
    Task<IEnumerable<Reader>> GetAllAsync();
    Task<Reader> GetReaderById(string id);
    Task<Reader> GetReaderByUsername(string username);
    Task<IEnumerable<ObjectId>> GetBooksFromReader(string readerId);
    Task AddBookToList(string id, string bookId);
    Task RemoveBookFromList(string id, string bookId);
}