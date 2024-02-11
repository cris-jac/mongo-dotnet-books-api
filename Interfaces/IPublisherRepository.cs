using API.Models;

namespace API.Interfaces;

public interface IPublisherRepository
{
    Task<IEnumerable<Publisher>> GetAllPublishers();
    Task AddPublisher(Publisher publisher);
    Task RemovePublisher(string id);
}