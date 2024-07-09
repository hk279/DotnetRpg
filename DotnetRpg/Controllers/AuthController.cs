using DotnetRpg.Dtos.User;
using DotnetRpg.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers;

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
        var response = await _authService.Register(newUser.Username, newUser.Password);
        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto loginDetails)
    {
        var response = await _authService.Login(loginDetails.Username, loginDetails.Password);
        return Ok(response);
    }

    [HttpGet("current-user")]
    public async Task<ActionResult<ServiceResponse<string>>> GetUserName()
    {
        var response = await _authService.GetUserName();
        return Ok(response);
    }
}
