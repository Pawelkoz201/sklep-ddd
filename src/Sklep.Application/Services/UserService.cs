using Sklep.Application.Common;
using Sklep.Application.DTOs;
using Sklep.Application.Ports;
using Sklep.Domain.Customers;
using Sklep.Domain.ValueObjects;

namespace Sklep.Application.Services;

public sealed class UserService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenGenerator _tokenGenerator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _users;

    public UserService(
        IUserRepository users,
        IPasswordHasher passwordHasher,
        ITokenGenerator tokenGenerator,
        IUnitOfWork unitOfWork)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<UserResponse>> GetUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _users.GetAllAsync(cancellationToken);
        return users.Select(MapUser).ToList();
    }

    public async Task<UserResponse> GetUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        return MapUser(user);
    }

    public async Task<UserResponse> RegisterAsync(RegisterRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.Password))
        {
            throw new BusinessRuleViolationException("Password is required.");
        }

        var email = new EmailAddress(request.Email);
        if (await _users.GetByEmailAsync(email, cancellationToken) is not null)
        {
            throw new BusinessRuleViolationException("User already exists.");
        }

        var user = new User(
            request.Username,
            email,
            _passwordHasher.HashPassword(request.Password));

        _users.Add(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return MapUser(user);
    }

    public async Task<LoginResponse> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        var email = new EmailAddress(request.Email);
        var user = await _users.GetByEmailAsync(email, cancellationToken)
            ?? throw new InvalidCredentialsException();

        if (!_passwordHasher.VerifyPassword(user.PasswordHash, request.Password))
        {
            throw new InvalidCredentialsException();
        }

        return new LoginResponse(_tokenGenerator.GenerateToken(user), user.Id);
    }

    public async Task UpdateUserAsync(int id, RegisterRequest request, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        user.ChangeProfile(request.Username, new EmailAddress(request.Email));
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            user.ChangePasswordHash(_passwordHasher.HashPassword(request.Password));
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAsync(int id, CancellationToken cancellationToken = default)
    {
        var user = await _users.GetByIdAsync(id, cancellationToken)
            ?? throw new NotFoundException("User not found.");

        _users.Remove(user);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private static UserResponse MapUser(User user)
    {
        return new UserResponse(user.Id, user.Username, user.Email.Value);
    }
}
