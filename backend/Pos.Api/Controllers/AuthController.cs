using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Pos.Api.Infrastructure;
using Pos.Api.Models;
using Pos.Api.Repositories;

namespace Pos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class AuthController : ControllerBase
{
    private readonly IAuthRepository authRepository;
    private readonly TokenService tokenService;

    public AuthController(IAuthRepository authRepository, TokenService tokenService)
    {
        this.authRepository = authRepository;
        this.tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest("Username and password are required.");
        }

        var user = await authRepository.LoginAsync(request.Username, request.Password, ct);
        if (user is null) return Unauthorized();

        var token = tokenService.GenerateToken(user);
        return Ok(new LoginResponse { Token = token, User = user });
    }
}
