namespace DotnetRpg.Data.Seeding;

public static class DataSeeder
{
    public static async Task SeedData(DataContext context)
    {
        var warriorSkills = WarriorSkillDataSeeder.GetWarriorSkills();
        
        await context.Skills.AddRangeAsync(warriorSkills);
        
        await context.SaveChangesAsync();
    }
}