using Microsoft.AspNetCore.Identity;
using Sklep.Application.Ports;

namespace Sklep.Infrastructure.Security;

public sealed class AspNetPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<object> _passwordHasher = new();
    private readonly object _passwordOwner = new();

    public string HashPassword(string password)
    {
        return _passwordHasher.HashPassword(_passwordOwner, password);
    }

    public bool VerifyPassword(string passwordHash, string password)
    {
        var result = _passwordHasher.VerifyHashedPassword(_passwordOwner, passwordHash, password);
        return result != PasswordVerificationResult.Failed;
    }
}
