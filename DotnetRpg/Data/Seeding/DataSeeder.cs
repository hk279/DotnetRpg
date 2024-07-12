using DotnetRpg.Models.Skills;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Data.Seeding;

public class DataSeeder
{
    private readonly DataContext _context;
    private readonly ILogger<DataSeeder> _logger;

    public DataSeeder(DataContext context, ILogger<DataSeeder> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task SeedData()
    {
        _logger.LogInformation("Seeding skills to database...");
        
        var allSkills = new List<Skill>();
        var existingSkills = await _context.Skills.ToListAsync();
        
        allSkills.AddRange(WarriorSkillDataSeeder.GetWarriorSkills());
        // TODO: Add other class skills
        
        var missingSkills = allSkills
            .Where(s => existingSkills.All(es => es.Name != s.Name && es.Rank != s.Rank))
            .ToList();

        foreach (var skill in missingSkills)
        {
            _logger.LogInformation("Adding missing skill: {SkillName} (Rank {SkillRank})", skill.Name, skill.Rank);
        }

        if (missingSkills.Count == 0)
        {
            _logger.LogInformation("All skills already seeded");
            return;
        }
        
        await _context.Skills.AddRangeAsync(missingSkills);
        
        await _context.SaveChangesAsync();
    }
}