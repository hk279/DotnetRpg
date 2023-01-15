using dotnet_rpg.Dtos.User;
using dotnet_rpg.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_rpg.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    public readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto newUser)
    {
        var response = await _authService.Register(new User { Username = newUser.Username }, newUser.Password);

        if (!response.Success) return BadRequest(response);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto loginDetails)
    {
        var response = await _authService.Login(loginDetails.Username, loginDetails.Password);

        if (!response.Success) return Unauthorized(response);
        return Ok(response);
    }
}
