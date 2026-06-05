namespace Sklep.Application.Ports;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string passwordHash, string password);
}
