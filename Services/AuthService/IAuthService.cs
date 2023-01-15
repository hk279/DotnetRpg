namespace dotnet_rpg.Services.AuthService;

public interface IAuthService
{
    Task<ServiceResponse<int>> Register(User user, string password);
    Task<ServiceResponse<LoginDetails>> Login(string username, string password);
    Task<bool> UserExists(string username);
}