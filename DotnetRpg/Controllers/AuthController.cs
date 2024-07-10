using DotnetRpg.Dtos.User;
using DotnetRpg.Services.AuthService;
using Microsoft.AspNetCore.Mvc;

namespace DotnetRpg.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(UserRegisterDto newUser)
    {
        await _authService.Register(newUser.Username, newUser.Password);
        return Ok();
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(UserLoginDto loginDetails)
    {
        var token = await _authService.Login(loginDetails.Username, loginDetails.Password);
        return Ok(token);
    }

    [HttpGet("current-user")]
    public async Task<ActionResult<string>> GetUserName()
    {
        var response = await _authService.GetUserName();
        return Ok(response);
    }
}