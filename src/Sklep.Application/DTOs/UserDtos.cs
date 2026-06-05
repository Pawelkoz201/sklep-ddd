namespace Sklep.Application.DTOs;

public sealed record RegisterRequest(string Username, string Email, string Password);

public sealed record LoginRequest(string Email, string Password);

public sealed record LoginResponse(string Token, int UserId);

public sealed record UserResponse(int Id, string Username, string Email);
