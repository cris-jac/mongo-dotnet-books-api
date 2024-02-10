using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Models;

public class Book
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime PublicationDate { get; set; }
    public List<ObjectId> AuthorIds { get; set; } = new List<ObjectId>();
    public List<ObjectId> CategoryIds { get; set; } = new List<ObjectId>();
    public List<ObjectId> PublisherIds { get; set; } = new List<ObjectId>();
    // public List<Bookcase> Bookcases { get; set; }
    // public List<Review> Reviews { get; set; }   
}