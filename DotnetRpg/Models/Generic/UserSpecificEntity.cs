using DotnetRpg.Models.Users;

namespace DotnetRpg.Models.Generic;

public abstract class UserSpecificEntity
{
    protected UserSpecificEntity() {}

    protected UserSpecificEntity(int userId)
    {
        UserId = userId;
    }

    public int Id { get; set; }
    public User User { get; set; } = null!;
    public int UserId { get; set; }
}