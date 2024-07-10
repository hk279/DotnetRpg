namespace DotnetRpg.Models;

public abstract class BaseEntity
{
    public int Id { get; set; }
    public int UserId { get; set; }
}