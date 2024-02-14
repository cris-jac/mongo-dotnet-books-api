using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private IMongoCollection<Publisher> _publisherCollection;

    public PublisherRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _publisherCollection = database.GetCollection<Publisher>(dbSettings.Value.PublisherCollectionName);
    }

    public async Task AddPublisher(Publisher publisher)
    {
        await _publisherCollection.InsertOneAsync(publisher);
    }

    public async Task<IEnumerable<Publisher>> GetPublishers()
    {
        return await _publisherCollection.Find(_ => true).ToListAsync();
    }

    public async Task<bool> PublisherExists(string id)
    {
        return await _publisherCollection.Find(p => p.Id == id).AnyAsync();
    }

    public async Task<bool> RemovePublisher(string id)
    {
        var deleted = await _publisherCollection.DeleteOneAsync(p => p.Id == id);
        return deleted.DeletedCount > 0;
    }

    // For BookController calls
    public async Task<List<string>> GetPublishersbyIds(List<string> publishersIds)
    {
        // 
        var publishersFilter = Builders<Publisher>.Filter.In(p => p.Id, publishersIds);

        var publishers = await _publisherCollection.Find(publishersFilter).ToListAsync();

        return publishers.Select(p => p.Name).ToList();
    }
}