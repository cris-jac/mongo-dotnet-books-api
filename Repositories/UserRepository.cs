using API.Configurations;
using API.DTO;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Repositories;

public class UserRepository : IUserRepository
{
    private IMongoCollection<User> _userCollection;
    public UserRepository(IOptions<DatabaseSettings> dbSettings)
    {
        var mongoClient = new MongoClient(dbSettings.Value.ConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(dbSettings.Value.DatabaseName);
        _userCollection = mongoDatabase.GetCollection<User>(dbSettings.Value.UserCollectionName);
    }
    
    public async Task<User> GetUserByEmail(string email)
    {
        return await _userCollection.Find(u => u.Email == email).FirstOrDefaultAsync();
    }
    
    public async Task AddUserAsync(User user)
    {
        await _userCollection.InsertOneAsync(user);
    }

    public async Task<IEnumerable<User>> GetUsers()
    {
        return await _userCollection.Find(_ => true).ToListAsync();
    }
}