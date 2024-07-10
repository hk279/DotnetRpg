using System.Security.Claims;
using DotnetRpg.Models.Exceptions;

namespace DotnetRpg.Services.UserProvider;

public class AuthenticatedUserProvider : IUserProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthenticatedUserProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int GetUserId()
    {
        var httpContext =
            _httpContextAccessor.HttpContext
            ?? throw new ArgumentNullException(nameof(_httpContextAccessor.HttpContext));
        var userId =
            httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedException("User not identified");
        
        return int.Parse(userId);
    }
}