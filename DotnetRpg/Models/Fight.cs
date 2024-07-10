namespace DotnetRpg.Models;

public class Fight : BaseEntity
{
    public Fight() {}
    
    public Fight(int userId, List<Character> characters)
    {
        UserId = userId;
        Characters = characters;
    }

    public List<Character> Characters { get; set; }
}

