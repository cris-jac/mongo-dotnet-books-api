using API.Configurations;
using API.Interfaces;
using API.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace API.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private IMongoCollection<Category> _categoryRepository;

    public CategoryRepository(
        IMongoDatabase database, 
        IOptions<DatabaseSettings> dbSettings
    )
    {
        _categoryRepository = database.GetCollection<Category>(dbSettings.Value.CategoryCollectionName);
    }

    public async Task AddCategory(Category category)
    {
        await _categoryRepository.InsertOneAsync(category);
    }

    public async Task<bool> CategoryExists(string id)
    {
        return await _categoryRepository.Find(c => c.Id == id).AnyAsync();
    }

    public async Task<IEnumerable<Category>> GetAllCategories()
    {
        return await _categoryRepository.Find(_=>true).ToListAsync();
    }

    public async Task<Category> GetCategoryById(string id)
    {
        return await _categoryRepository.Find(c => c.Id == id).FirstOrDefaultAsync();
    }

    public async Task<bool> RemoveCategory(string id)
    {
        var deleted = await _categoryRepository.DeleteOneAsync(c => c.Id==id);
        return deleted.DeletedCount > 0;
    }

    public async Task UpdateCategory(string id, Category category)
    {
        await _categoryRepository.ReplaceOneAsync(c => c.Id == id, category);
    }
}