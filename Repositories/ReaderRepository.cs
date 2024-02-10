using System.Xml.Schema;
using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories;

public class ReaderRepository : IReaderRepository
{
    private IMongoCollection<Reader> _readerRepository;
    public ReaderRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _readerRepository = mongoDatabase.GetCollection<Reader>(dbSettings.Value.ReaderCollectionName);
    }

    public async Task AddBookToList(string id, string bookId)
    {
        var reader = await _readerRepository.Find(r => r.Id == id).FirstOrDefaultAsync();
        ObjectId bookObjectId = ObjectId.Parse(bookId);
        reader.BookIds.Add(bookObjectId);
        throw new NotImplementedException();        
    }

    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        return await _readerRepository.Find(_ => true).ToListAsync();
    }

    public async Task<IEnumerable<ObjectId>> GetBooksFromReader(string readerId)
    {
        var reader = await _readerRepository.Find(r => r.Id == readerId).FirstOrDefaultAsync();
        var readerBooks = reader.BookIds;
        throw new NotImplementedException();
    }

    public async Task<Reader> GetReaderById(string id)
    {
        return await _readerRepository.Find(r => r.Id ==  id).FirstOrDefaultAsync();
    }

    public Task<Reader> GetReaderByUsername(string username)
    {
        throw new NotImplementedException();
    }

    public Task RemoveBookFromList(string id, string bookId)
    {
        throw new NotImplementedException();
    }
}