using API.Models;

namespace API.Interfaces;

public interface INationalityRepository
{
    Task<IEnumerable<Nationality>> GetAllNationalities();
    Task<bool> NationalityExists(string id);
    Task<Nationality> GetNationalityById(string id);
    Task AddNationality(Nationality nationality);
    Task<bool> RemoveNationality(string id);
    Task UpdateNationality(string id, Nationality nationality);
}