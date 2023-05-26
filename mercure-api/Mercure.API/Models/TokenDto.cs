namespace Mercure.API.Models;

/// <summary>
/// Token dto
/// </summary>
public class TokenDto
{
    public TokenDto(string token)
    {
        this.token = token;
    }

    public string token { get; set; }
}