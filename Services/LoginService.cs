using System.Security.Cryptography;
using System.Text;
using API.Models;

namespace API.Services;

public class LoginService
{
    public bool Login(User user, string passwordToCheck)
    {
        using var hmac = new HMACSHA256(user.PasswordSalt);

        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(passwordToCheck));

        for (int i = 0; i < computedHash.Length; i++)
        {
            if (computedHash[i] != user.PasswordHash[i]) return false;
        }

        return true;
    }
}