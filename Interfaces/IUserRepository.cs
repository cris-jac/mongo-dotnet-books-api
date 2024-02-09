using API.Models;

namespace API.Interfaces;

public interface IUserRepository
{
    Task<User> GetUserByEmail(string email);
    Task AddUserAsync(User user);
}