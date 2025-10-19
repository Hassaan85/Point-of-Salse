namespace Pos.Api.Models;

public sealed class UserInfo
{
    public int UserId { get; init; }
    public string Username { get; init; } = string.Empty;
    public string FullName { get; init; } = string.Empty;
    public string RoleName { get; init; } = string.Empty;
}
