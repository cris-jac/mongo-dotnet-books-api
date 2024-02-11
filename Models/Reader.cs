using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;

public class Reader : Person
{
    public string Id { get; set; }
    public string Username { get; set; }
    public List<ObjectId> BookIds { get; set; }
}