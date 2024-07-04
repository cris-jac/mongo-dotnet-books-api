namespace API.Models;

public class ApiResponse
{
    public int StatusCode { get; init; }
    public bool IsSuccess { get; init; }
    public object Result { get; init; } = new object();
    public object Error { get; init; } = new object();
}