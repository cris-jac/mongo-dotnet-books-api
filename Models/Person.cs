using MongoDB.Bson;

namespace API.Models;

public class Person
{
    public string Name { get; set; }
    public ObjectId NationalityId { get; set; }
}