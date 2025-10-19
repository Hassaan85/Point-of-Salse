namespace Pos.Api.Models;

public sealed class LoginRequest
{
    public string Username { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
}

public sealed class LoginResponse
{
    public string Token { get; init; } = string.Empty;
    public UserInfo User { get; init; } = new();
}
