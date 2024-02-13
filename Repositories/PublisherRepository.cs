using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Repositories;

public class PublisherRepository : IPublisherRepository
{
    private IMongoCollection<Publisher> _publisherRepository;

    public PublisherRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _publisherRepository = database.GetCollection<Publisher>(dbSettings.Value.PublisherCollectionName);
    }

    public async Task AddPublisher(Publisher publisher)
    {
        await _publisherRepository.InsertOneAsync(publisher);
    }

    public async Task<IEnumerable<Publisher>> GetAllPublishers()
    {
        return await _publisherRepository.Find(_=>true).ToListAsync();
    }

    public async Task<bool> PublisherExists(string id)
    {
        return await _publisherRepository.Find(p => p.Id == id).AnyAsync();
    }

    public async Task<bool> RemovePublisher(string id)
    {
        var deleted = await _publisherRepository.DeleteOneAsync(p => p.Id == id);
        return deleted.DeletedCount > 0;
    }
}