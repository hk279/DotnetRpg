namespace DotnetRpg.Services.AuthService;

public interface IAuthService
{
    Task<string> GetUserName();
    Task Register(string userName, string password);
    Task<string> Login(string username, string password);
}
