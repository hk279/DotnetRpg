using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using dotnet_rpg.Data;
using dotnet_rpg.Models.Exceptions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dotnet_rpg.Services.AuthService;

public class AuthService : IAuthService
{
    public readonly DataContext _context;
    public readonly IConfiguration _config;

    public AuthService(DataContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<ServiceResponse<LoginDetails>> Login(string username, string password)
    {
        var response = new ServiceResponse<LoginDetails>();

        var user =
            await _context.Users.FirstOrDefaultAsync(
                u => u.Username.ToLower().Equals(username.ToLower())
            ) ?? throw new UnauthorizedException("Login failed. User not found.");

        if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
        {
            throw new UnauthorizedException("Login failed. Incorrect password");
        }

        response.Data = new LoginDetails(user.Username, CreateToken(user));

        return response;
    }

    public async Task<ServiceResponse<int>> Register(string userName, string password)
    {
        var response = new ServiceResponse<int>();

        if (await UserExists(userName))
        {
            throw new ConflictException("Username is taken");
        }

        var user = new User { Username = userName };

        CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
        user.PasswordHash = passwordHash;
        user.PasswordSalt = passwordSalt;

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        response.Data = user.Id;

        return response;
    }

    public async Task<bool> UserExists(string username)
    {
        var userExists = await _context.Users.AnyAsync(
            u => u.Username.ToLower() == username.ToLower()
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

        var tokenSecret =
            _config.GetSection("TokenSettings:Secret").Value
            ?? throw new ArgumentException("Token secret not found");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenSecret));
        var signingCredentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha512Signature
        );
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
}
