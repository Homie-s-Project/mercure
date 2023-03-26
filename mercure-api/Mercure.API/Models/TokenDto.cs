namespace Mercure.API.Models;

public class TokenDto
{
    public TokenDto(string token)
    {
        this.token = token;
    }

    public string token { get; set; }
}