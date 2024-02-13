using API.Models;

namespace API.Interfaces;

public interface IPublisherRepository
{
    Task<bool> PublisherExists(string id);
    Task<IEnumerable<Publisher>> GetAllPublishers();
    Task AddPublisher(Publisher publisher);
    Task<bool> RemovePublisher(string id);
}