using API.Models;

namespace API.Interfaces;

public interface IPublisherRepository
{
    Task<bool> PublisherExists(string id);
    Task<IEnumerable<Publisher>> GetPublishers();
    Task AddPublisher(Publisher publisher);
    Task<bool> RemovePublisher(string id);
    //
    Task<List<string>> GetPublishersbyIds(List<string> publishersIds);
}