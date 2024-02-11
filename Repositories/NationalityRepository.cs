using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
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

    public Task AddNationality(Nationality nationality)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Nationality>> GetAllNationalities()
    {
        throw new NotImplementedException();
    }

    public Task<Nationality> GetNationalityById(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> NationalityExists(string id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> RemoveNationality(string id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateNationality(string id, Nationality nationality)
    {
        throw new NotImplementedException();
    }
}