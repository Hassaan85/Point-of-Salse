using Pos.Api.Models;

namespace Pos.Api.Repositories;

public interface IAuthRepository
{
    Task<UserInfo?> LoginAsync(string username, string password, CancellationToken ct);
    Task<int> CreateUserAsync(string username, string password, string fullName, string roleName, CancellationToken ct);
}
