using MongoDB.Bson;

namespace API.Models;

public class Reader : User
{
    public List<ObjectId> BookIds { get; set; }
}