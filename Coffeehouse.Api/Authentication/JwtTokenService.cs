using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Coffeehouse.Api.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Coffeehouse.Api.Authentication;

public class JwtTokenService
{
    private readonly AuthenticationOptions _options;

    public JwtTokenService(IOptions<AuthenticationOptions> options)
    {
        _options = options.Value;
    }

    public LoginResult GenerateToken(DemoUser user)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SigningKey));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var issuedAt = DateTime.UtcNow;
        var expiresAt = issuedAt.AddMinutes(_options.AccessTokenMinutes);

        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString("D")),
            new Claim(JwtRegisteredClaimNames.Name, user.Username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")),
            new Claim(JwtRegisteredClaimNames.Iat, EpochSeconds(issuedAt).ToString(), ClaimValueTypes.Integer64),
        };

        var token = new JwtSecurityToken(
            issuer: _options.Issuer,
            audience: _options.Audience,
            claims: claims,
            notBefore: issuedAt,
            expires: expiresAt,
            signingCredentials: credentials);

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new LoginResult(tokenString, _options.AccessTokenMinutes * 60);
    }

    public static string GenerateRandomSigningKey()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }

    private static long EpochSeconds(DateTime time) => new DateTimeOffset(time).ToUnixTimeSeconds();

    public record LoginResult(string Token, int ExpiresInSeconds);
}
