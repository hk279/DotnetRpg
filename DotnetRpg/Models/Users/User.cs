using DotnetRpg.Models.Characters;

namespace DotnetRpg.Models.Users;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = null!;
    public byte[] PasswordSalt { get; set; } = null!;
    public List<Character> Characters { get; set; } = [];
}