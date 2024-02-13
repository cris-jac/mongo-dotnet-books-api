using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Repositories;

public class AuthorRepository : IAuthorRepository
{
    private IMongoCollection<Author> _authorRepository;

    public AuthorRepository(
        IMongoDatabase database,
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _authorRepository = database.GetCollection<Author>(dbSettings.Value.AuthorCollectionName);
    }

    public async Task AddAuthor(Author author)
    {
        await _authorRepository.InsertOneAsync(author);
    }

    public async Task<bool> AuthorExists(string id)
    {
        return await _authorRepository.Find(a => a.Id == id).AnyAsync();
    }

    public async Task<IEnumerable<Author>> GetAllAuthors()
    {
        return await _authorRepository.Find(_=>true).ToListAsync();
    }

    public async Task<Author> GetAuthorById(string id)
    {
        return await _authorRepository.Find(a => a.Id == id).FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<Author>> GetAuthorsByNationalityId(string nationalityId)
    {
        ObjectId nationalityObjectId = ObjectId.Parse(nationalityId);
        var filter = Builders<Author>.Filter.Eq(a => a.NationalityId, nationalityObjectId);
        return await _authorRepository.Find(filter).ToListAsync();
    }

    public async Task<bool> RemoveAuthor(string id)
    {
        var deleted = await _authorRepository.DeleteOneAsync(a => a.Id == id);
        return deleted.DeletedCount > 0;
    }

    public async Task UpdateAuthor(string id, Author author)
    {
        await _authorRepository.ReplaceOneAsync(a => a.Id == id, author);
    }

    public async Task<bool> UpdateAuthorNationality(string authorId, string nationalityId)
    {
        var updateDefinition = Builders<Author>.Update.Set(a => a.NationalityId, ObjectId.Parse(nationalityId));
        var updated = await _authorRepository.UpdateOneAsync(a => a.Id == authorId, updateDefinition);
        return updated.ModifiedCount > 0;
    }
}