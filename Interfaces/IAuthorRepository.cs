using API.Models;

namespace API.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAuthors();
    Task<bool> AuthorExists(string id);
    Task<Author> GetAuthorById(string id);
    Task AddAuthor(Author author);
    Task<bool> RemoveAsync(string id);
    Task UpdateAuthor(string id, Author author);
}