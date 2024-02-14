namespace API.DTO;

public class GetReaderDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public List<string> Books { get; set; }
    public string Nationality { get; set; }
}