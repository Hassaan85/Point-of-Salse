using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Pos.Api.Models;

namespace Pos.Api.Infrastructure;

public sealed class TokenService
{
    private readonly string issuer;
    private readonly string audience;
    private readonly SymmetricSecurityKey signingKey;

    public TokenService(string issuer, string audience, SymmetricSecurityKey signingKey)
    {
        this.issuer = issuer;
        this.audience = audience;
        this.signingKey = signingKey;
    }

    public string GenerateToken(UserInfo userInfo, TimeSpan? lifetime = null)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, userInfo.UserId.ToString()),
            new(ClaimTypes.Name, userInfo.Username),
            new(ClaimTypes.GivenName, userInfo.FullName),
            new(ClaimTypes.Role, userInfo.RoleName)
        };

        var creds = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            notBefore: DateTime.UtcNow,
            expires: DateTime.UtcNow.Add(lifetime ?? TimeSpan.FromHours(8)),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
