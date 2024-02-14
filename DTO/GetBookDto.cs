namespace API.DTO;

public class GetBookDto
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string PublicationYear { get; set; }
    public List<string> Authors { get; set; }
}