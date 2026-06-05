using Sklep.Domain.Customers;

namespace Sklep.Application.Ports;

public interface ITokenGenerator
{
    string GenerateToken(User user);
}
