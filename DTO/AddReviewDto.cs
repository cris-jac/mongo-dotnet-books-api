using MongoDB.Bson;

namespace API.DTO;

public class AddReviewDto
{
    public string ReaderId { get; set; }
    public string BookId { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}