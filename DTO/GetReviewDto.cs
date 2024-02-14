namespace API.DTO;

public class GetReviewDto
{
    public string Id { get; set; }
    public string Reader { get; set; }
    public string Book { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }
}