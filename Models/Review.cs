using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;

public class Review
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public ObjectId ReaderId { get; set; }
    public ObjectId BookId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}