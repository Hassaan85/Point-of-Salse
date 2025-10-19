using System.Data;
using Dapper;
using Pos.Api.Models;

namespace Pos.Api.Repositories;

public sealed class AuthRepository : IAuthRepository
{
    private readonly IDbConnection connection;

    public AuthRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<UserInfo?> LoginAsync(string username, string password, CancellationToken ct)
    {
        var result = await connection.QuerySingleOrDefaultAsync<UserInfo>(
            new CommandDefinition(
                commandText: "dbo.spAuth_Login",
                parameters: new { Username = username, Password = password },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
        return result;
    }

    public async Task<int> CreateUserAsync(string username, string password, string fullName, string roleName, CancellationToken ct)
    {
        var newId = await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(
                commandText: "dbo.spAuth_CreateUser",
                parameters: new { Username = username, Password = password, FullName = fullName, RoleName = roleName },
                commandType: CommandType.StoredProcedure,
                cancellationToken: ct
            ));
        return newId;
    }
}
