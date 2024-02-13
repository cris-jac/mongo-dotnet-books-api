using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories;

public class NationalityRepository : INationalityRepository
{
    private IMongoCollection<Nationality> _nationalityRepository;

    public NationalityRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _nationalityRepository = database.GetCollection<Nationality>(dbSettings.Value.NationalityCollectionName);
    }

    public async Task AddNationality(Nationality nationality)
    {
        await _nationalityRepository.InsertOneAsync(nationality);
    }

    public async Task<IEnumerable<Nationality>> GetAllNationalities()
    {
        return await _nationalityRepository.Find(_ => true).ToListAsync();
    }

    public async Task<Nationality> GetNationalityById(string id)
    {
        return await _nationalityRepository.Find(n => n.Id == id).FirstOrDefaultAsync();
    }

    public async Task<Nationality> GetNationalityByName(string name)
    {
        var filter = Builders<Nationality>.Filter.Regex("Name", new BsonRegularExpression(name, "i"));
        return await _nationalityRepository.Find(filter).FirstOrDefaultAsync();
    }

    public async Task<bool> NationalityExists(string id)
    {
        return await _nationalityRepository.Find(n => n.Id == id).AnyAsync();
    }

    public async Task<bool> RemoveNationality(string id)
    {
        var deleted = await _nationalityRepository.DeleteOneAsync(n => n.Id == id);
        return deleted.DeletedCount > 0;
    }

    public async Task UpdateNationality(string id, Nationality nationality)
    {
        await _nationalityRepository.ReplaceOneAsync(n => n.Id == id, nationality);
    }
}