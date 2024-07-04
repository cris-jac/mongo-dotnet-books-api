using System.Net;
using System.Text.Json;

namespace API.Models;

public class ErrorResponse
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Message { get; set; } = string.Empty;
    // public string ExceptionType { get; set; } = string.Empty;
    public HttpStatusCode StatusCode { get; set; }
    public string StackTrace { get; set; } = string.Empty;

    public string SerializeError()
    {
        return JsonSerializer.Serialize(this);
    }
}