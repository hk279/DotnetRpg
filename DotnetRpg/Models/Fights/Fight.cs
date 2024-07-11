using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Generic;

namespace DotnetRpg.Models.Fights;

public class Fight : BaseEntity
{
    public Fight() {}
    
    public Fight(int userId, List<Character> characters): base(userId)
    {
        Characters = characters;
    }

    public List<Character> Characters { get; set; } = null!;
}

