using API.Models;

namespace API.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllCategories();
    Task<bool> CategoryExists(string id);
    Task<Category> GetCategoryById(string id);
    Task AddCategory(Category category);
    Task<bool> RemoveCategory(string id);
    Task UpdateCategory(string id, Category category);    
}