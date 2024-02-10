using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;

public class Reader : User
{
    public List<ObjectId> BookIds { get; set; }
}