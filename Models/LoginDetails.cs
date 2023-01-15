namespace dotnet_rpg.Models;

public class LoginDetails
{
    public LoginDetails(string username, string token)
    {
        Username = username;
        Token = token;
    }

    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
}
