using Sklep.Domain.Common;
using Sklep.Domain.ValueObjects;

namespace Sklep.Domain.Customers;

public sealed class User : Entity
{
    private User()
    {
        Username = string.Empty;
        Email = new EmailAddress("unknown@example.com");
        PasswordHash = string.Empty;
    }

    public User(string username, EmailAddress email, string passwordHash)
    {
        ChangeProfile(username, email);
        ChangePasswordHash(passwordHash);
    }

    public string Username { get; private set; } = string.Empty;
    public EmailAddress Email { get; private set; } = new("unknown@example.com");
    public string PasswordHash { get; private set; } = string.Empty;

    public void ChangeProfile(string username, EmailAddress email)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            throw new DomainException("Username is required.");
        }

        Username = username.Trim();
        Email = email ?? throw new DomainException("Email is required.");
    }

    public void ChangePasswordHash(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
        {
            throw new DomainException("Password hash is required.");
        }

        PasswordHash = passwordHash;
    }
}
