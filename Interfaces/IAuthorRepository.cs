using API.Models;

namespace API.Interfaces;

public interface IAuthorRepository
{
    Task<IEnumerable<Author>> GetAllAuthors();
    Task<Author> GetAuthorById(string id);
    Task<IEnumerable<Author>> GetAuthorsByNationalityId(string nationalityId);
    Task<bool> AuthorExists(string id);
    Task AddAuthor(Author author);
    Task<bool> RemoveAuthor(string id);
    Task UpdateAuthor(string id, Author author);
    Task<bool> UpdateAuthorNationality(string authorId, string nationalityId);
}