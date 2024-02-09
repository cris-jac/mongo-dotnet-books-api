namespace API.DTO;

public class LoginResponseDto
{
    public string Name { get; set; }
    public string Username { get; set; }
    public string JwtToken { get; set; }
}