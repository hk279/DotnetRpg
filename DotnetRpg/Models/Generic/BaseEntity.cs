namespace DotnetRpg.Models.Generic;

public abstract class BaseEntity
{
    protected BaseEntity() {}

    protected BaseEntity(int userId)
    {
        UserId = userId;
    }

    public int Id { get; set; }
    public int UserId { get; set; }
}