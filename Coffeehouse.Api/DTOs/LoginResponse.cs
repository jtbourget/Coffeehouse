namespace Coffeehouse.Api.DTOs;

public class LoginResponse
{
    public required string AccessToken { get; set; }
    public required string TokenType { get; set; }
    public required int ExpiresIn { get; set; }
}
