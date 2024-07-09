namespace DotnetRpg.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<string>> GetUserName();
    Task<ServiceResponse<int>> Register(string userName, string password);
    Task<ServiceResponse<LoginDetails>> Login(string username, string password);
    Task<bool> UserExists(string username);
}
