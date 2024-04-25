using DotnetRpg.Data.Seeding;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Fight>()
                .HasMany(f => f.Characters)
                .WithOne(c => c.Fight)
                .HasForeignKey(c => c.FightId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Item>()
                .HasDiscriminator(i => i.Type)
                .HasValue<ArmorPiece>(ItemType.ArmorPiece)
                .HasValue<Weapon>(ItemType.Weapon);

            modelBuilder
                .Entity<Character>()
                .HasMany(c => c.SkillInstances)
                .WithOne(c => c.Character);

            modelBuilder
                .Entity<Character>()
                .HasMany(c => c.StatusEffectInstances)
                .WithOne(c => c.Character);

            modelBuilder
                .Entity<StatusEffect>()
                .HasMany(s => s.StatusEffectInstances)
                .WithOne(s => s.StatusEffect);

            modelBuilder
                .Entity<Skill>()
                .HasOne(s => s.StatusEffect)
                .WithOne(s => s.Skill)
                .HasForeignKey<StatusEffect>(s => s.SkillId);

            // Seed skills
            // TODO: Add other classes
            var (warriorSkills, warriorSkillStatusEffects) = SkillDataSeeder.GetWarriorSkills();

            modelBuilder.Entity<Skill>().HasData(warriorSkills);
            modelBuilder.Entity<StatusEffect>().HasData(warriorSkillStatusEffects);
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<ArmorPiece> ArmorPieces { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
    }
}
