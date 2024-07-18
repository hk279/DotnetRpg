using DotnetRpg.Dtos.User;

namespace DotnetRpg.Services.AuthService;

public interface IAuthService
{
    Task<string> GetUserName();
    Task Register(string userName, string password);
    Task<LoginResponseDto> Login(string username, string password);
}
