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
    private IMongoCollection<Reader> _readerCollection;
    public ReaderRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        // var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        // var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        // _readerCollection = mongoDatabase.GetCollection<Reader>(dbSettings.Value.ReaderCollectionName);
        _readerCollection = database.GetCollection<Reader>(dbSettings.Value.ReaderCollectionName);
    }

    public async Task<IEnumerable<Reader>> GetAllAsync()
    {
        return await _readerCollection.Find(_ => true).ToListAsync();
    }

    // public async Task<IEnumerable<ObjectId>> GetBooksFromReader(string readerId)
    // {
    //     var reader = await _readerCollection.Find(r => r.Id == readerId).FirstOrDefaultAsync();
    //     return reader.BookIds;
    // }

    public async Task<Reader> GetReaderById(string id)
    {
        return await _readerCollection.Find(r => r.Id ==  id).FirstOrDefaultAsync();
    }

    public async Task<Reader> GetReaderByUsername(string username)
    {
        var filter = Builders<Reader>.Filter.Regex("Username", new BsonRegularExpression(username, "i"));
        return await _readerCollection.Find(filter).FirstOrDefaultAsync();
    }

    public async Task AddReader(Reader reader)
    {
        await _readerCollection.InsertOneAsync(reader);
    }

    public async Task<bool> AddBookToList(string readerId, string bookId)
    {
        // var reader = await _readerCollection.Find(r => r.Id == id).FirstOrDefaultAsync();
        // ObjectId bookObjectId = ObjectId.Parse(bookId);
        // reader.BookIds.Add(bookObjectId);
        var readerFiltered = Builders<Reader>.Filter.Eq(r => r.Id, readerId);

        var bookToAdd = Builders<Reader>.Update.AddToSet(r => r.BookIds, ObjectId.Parse(bookId));

        var updated = await _readerCollection.UpdateOneAsync(readerFiltered, bookToAdd);

        return updated.ModifiedCount > 0;
    }

    public async Task<bool> RemoveBookFromList(string readerId, string bookId)
    {
        var readerFiltered = Builders<Reader>.Filter.Eq(r => r.Id, readerId);

        var bookToRemove = Builders<Reader>.Update.Pull(r => r.BookIds, ObjectId.Parse(bookId));

        var updated = await _readerCollection.UpdateOneAsync(readerFiltered, bookToRemove);

        return updated.ModifiedCount > 0;
    }

    public async Task<bool> UpdateReaderNationality(string readerId, string nationalityId)
    {
        var updatedDef = Builders<Reader>.Update.Set(r => r.NationalityId, ObjectId.Parse(nationalityId));
        var updated = await _readerCollection.UpdateOneAsync(r => r.Id == readerId, updatedDef);
        return updated.ModifiedCount > 0;
    }

    public async Task<bool> ReaderExists(string id)
    {
        return await _readerCollection.Find(r => r.Id == id).AnyAsync();
    }
}