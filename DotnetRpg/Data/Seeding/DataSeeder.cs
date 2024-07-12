using DotnetRpg.Models.Skills;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Data.Seeding;

public static class DataSeeder
{
    public static async Task SeedData(DataContext context, ILogger logger)
    {
        logger.LogInformation("Seeding skills to database...");
        
        var allSkills = new List<Skill>();
        var existingSkills = await context.Skills.ToListAsync();
        
        allSkills.AddRange(WarriorSkillDataSeeder.GetWarriorSkills());
        // TODO: Add other class skills
        
        var missingSkills = allSkills
            .Where(s => existingSkills.All(es => es.Name != s.Name && es.Rank != s.Rank))
            .ToList();

        foreach (var skill in missingSkills)
        {
            logger.LogInformation("Adding missing skill: {SkillName} (Rank {SkillRank})", skill.Name, skill.Rank);
        }

        if (missingSkills.Count == 0)
        {
            logger.LogInformation("All skills already seeded");
            return;
        }
        
        await context.Skills.AddRangeAsync(missingSkills);
        
        await context.SaveChangesAsync();
    }
}