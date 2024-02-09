using System.Security.Cryptography;
using System.Text;
using API.DTO;
using API.Models;

namespace API.Services;

public class RegisterService
{

    public User Register(RegisterDto request)
    {
        using var hmac = new HMACSHA256();

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            Name = request.Name,
            PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(request.Password)),
            PasswordSalt = hmac.Key
        };

        return user;
    }
}