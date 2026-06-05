using Sklep.Domain.Customers;
using Sklep.Domain.ValueObjects;

namespace Sklep.Application.Ports;

public interface IUserRepository
{
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<User?> GetByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);
    void Add(User user);
    void Remove(User user);
}
