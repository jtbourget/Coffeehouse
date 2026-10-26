using System.Security.Claims;
using Coffeehouse.Api.Authentication;
using Coffeehouse.Api.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Coffeehouse.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly DemoUserStore _userStore;
    private readonly JwtTokenService _tokenService;
    private readonly LoginRateLimiter _rateLimiter;

    public AuthController(DemoUserStore userStore, JwtTokenService tokenService, LoginRateLimiter rateLimiter)
    {
        _userStore = userStore;
        _tokenService = tokenService;
        _rateLimiter = rateLimiter;
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        if (!_rateLimiter.IsAllowed(HttpContext.Features))
        {
            Response.Headers.Append("Retry-After", "60");
            return StatusCode(StatusCodes.Status429TooManyRequests);
        }

        if (!_userStore.TryGetUser(request.Username, out var user) ||
            _userStore.VerifyPassword(user, request.Password) == PasswordVerificationResult.Failed)
        {
            return Unauthorized();
        }

        var result = _tokenService.GenerateToken(user);
        Response.Headers.CacheControl = "no-store";
        return Ok(new LoginResponse
        {
            AccessToken = result.Token,
            TokenType = "Bearer",
            ExpiresIn = result.ExpiresInSeconds,
        });
    }

    [HttpGet("me")]
    [Authorize]
    public IActionResult Me()
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized();
        }

        return Ok(new { username });
    }
}
