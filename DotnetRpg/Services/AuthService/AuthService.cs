using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using DotnetRpg.Data;
using DotnetRpg.Dtos.Users;
using DotnetRpg.Models.Exceptions;
using DotnetRpg.Models.Users;
using DotnetRpg.Services.UserProvider;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DotnetRpg.Services.AuthService;

public class AuthService : IAuthService
{
    public const int UserNameMinLength = 3;
    public const int UserNameMaxLength = 12;
    public const int PasswordMinLength = 5;
    public const int PasswordMaxLength = 16;

    private readonly DataContext _context;
    private readonly IConfiguration _config;
    private readonly IUserProvider _userProvider;

    public AuthService(DataContext context, IConfiguration config, IUserProvider userProvider)
    {
        _context = context;
        _config = config;
        _userProvider = userProvider;
    }

    public async Task<string> GetUserName()
    {
        var userId = _userProvider.GetUserId();
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new NotFoundException($"User not found");

        return user.Username;
    }

    public async Task<LoginResponseDto> Login(string username, string password)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username)
                   ?? throw new UnauthorizedException($"Login failed. User not found with name '{username}'.");

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedException("Login failed. Incorrect password");
        }

        var token = CreateToken(user);

        return new LoginResponseDto(token, user.Username);
    }

    public async Task Register(string userName, string password)
    {
        await ValidateRegistration(userName, password);

        var user = new User { Username = userName };

        CreatePasswordHash(password, out var passwordHash, out var passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    private async Task<bool> UserExists(string username)
    {
        var userExists = await _context.Users.AnyAsync(
            u => u.Username == username
        );
        return userExists;
    }

    private static void CreatePasswordHash(
        string password,
        out byte[] passwordHash,
        out byte[] passwordSalt
    )
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(
        string password,
        byte[] passwordHash,
        byte[] passwordSalt
    )
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computeHash.SequenceEqual(passwordHash);
    }

    private string CreateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Username)
        };

        var tokenSecret = _config.GetSection("TokenSettings:Secret").Value
                          ?? throw new ArgumentException("Token secret not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
        var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddDays(1),
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    private async Task ValidateRegistration(string userName, string password)
    {
        if (userName.Length is < UserNameMinLength or > UserNameMaxLength)
        {
            throw new BadRequestException(
                $"Username has to be between {UserNameMinLength} and {UserNameMaxLength} characters long");
        }

        if (password.Length is < PasswordMinLength or > PasswordMaxLength)
        {
            throw new BadRequestException(
                $"Password has to be between {PasswordMinLength} and {PasswordMaxLength} characters long");
        }

        if (await UserExists(userName))
        {
            throw new ConflictException("Username is taken");
        }
    }
}