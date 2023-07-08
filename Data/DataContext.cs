using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                // Warrior
                new Skill { Id = 1, Name = "Charge", Damage = 10, DamageType = SkillDamageType.Physical, CharacterClass = CharacterClass.Warrior },
                new Skill { Id = 2, Name = "Rend", Damage = 5, DamageType = SkillDamageType.Physical, CharacterClass = CharacterClass.Warrior },
                new Skill { Id = 3, Name = "Enrage", Damage = 0, DamageType = SkillDamageType.Physical, TargetType = SkillTargetType.Self, CharacterClass = CharacterClass.Warrior },
                new Skill { Id = 4, Name = "Skillful Strike", Damage = 20, DamageType = SkillDamageType.Physical, CharacterClass = CharacterClass.Warrior },
                // Mage
                new Skill { Id = 5, Name = "Arcane Barrier", Damage = 0, DamageType = SkillDamageType.Magic, TargetType = SkillTargetType.Friendly, CharacterClass = CharacterClass.Mage },
                new Skill { Id = 6, Name = "Ice Lance", Damage = 20, DamageType = SkillDamageType.Magic, CharacterClass = CharacterClass.Mage },
                new Skill { Id = 7, Name = "Combustion", Damage = 5, DamageType = SkillDamageType.Magic, CharacterClass = CharacterClass.Mage },
                new Skill { Id = 8, Name = "Lightning Storm", Damage = 10, DamageType = SkillDamageType.Magic, CharacterClass = CharacterClass.Mage },
                // Priest
                new Skill { Id = 9, Name = "Battle Meditation", Damage = 0, DamageType = SkillDamageType.Magic, TargetType = SkillTargetType.Self, CharacterClass = CharacterClass.Priest },
                new Skill { Id = 10, Name = "Miraclous Touch", Damage = 0, Healing = 20, DamageType = SkillDamageType.Magic, TargetType = SkillTargetType.Friendly, CharacterClass = CharacterClass.Priest },
                new Skill { Id = 11, Name = "Holy Smite", Damage = 20, DamageType = SkillDamageType.Magic, CharacterClass = CharacterClass.Priest },
                new Skill { Id = 12, Name = "Cleansing Pain", Damage = 5, DamageType = SkillDamageType.Magic, CharacterClass = CharacterClass.Priest }
            );

            modelBuilder.Entity<Fight>()
                .HasMany(f => f.Characters)
                .WithOne(c => c.Fight)
                .HasForeignKey(c => c.FightId)
                .OnDelete(DeleteBehavior.SetNull);
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Fight> Fights { get; set; }
    }
}