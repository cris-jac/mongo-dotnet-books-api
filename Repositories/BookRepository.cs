using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories;

public class BookRepository : IBookRepository
{
    private IMongoCollection<Book> _bookRepository;
    public BookRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _bookRepository = mongoDatabase.GetCollection<Book>(dbSettings.Value.BookCollectionName);
    }
    public async Task AddAsync(Book book)
    {
        await _bookRepository.InsertOneAsync(book);
    }

    public async Task<bool> BookExists(string id)
    {
        return await _bookRepository.Find(b => b.Id == id).AnyAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        return await _bookRepository.Find(_ => true).ToListAsync(); 
    }

    public async Task<Book> GetBookById(string id)
    {
        return await _bookRepository.Find(b => b.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByAuthor(string authorId)
    {
        ObjectId authorObjectId = ObjectId.Parse(authorId);
        return await _bookRepository.Find(b => b.AuthorIds.Contains(authorObjectId)).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByCategory(string categoryId)
    {
        ObjectId categoryObjectId = ObjectId.Parse(categoryId);
        return await _bookRepository.Find(b => b.CategoryIds.Contains(categoryObjectId)).ToListAsync();
    }

    public async Task<IEnumerable<Book>> GetBooksByPublisher(string publisherId)
    {
        ObjectId publisherObjectId = ObjectId.Parse(publisherId);
        return await _bookRepository.Find(b => b.PublisherIds.Contains(publisherObjectId)).ToListAsync();
    }

    public async Task RemoveAsync(string id)
    {
        await _bookRepository.DeleteOneAsync(b => b.Id == id);
    }

    public async Task UpdateAsync(string id, Book book)
    {
        await _bookRepository.ReplaceOneAsync(b => b.Id == id, book);
    }
}