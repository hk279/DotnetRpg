using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Entity<Skill>()
                .HasData(
                    // Warrior
                    new Skill
                    {
                        Id = 1,
                        Name = "Charge",
                        Damage = 10,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 15,
                        Cooldown = 5
                    },
                    new Skill
                    {
                        Id = 2,
                        Name = "Rend",
                        Damage = 5,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 10,
                        Cooldown = 5
                    },
                    new Skill
                    {
                        Id = 3,
                        Name = "Enrage",
                        Damage = 0,
                        DamageType = DamageType.Physical,
                        TargetType = SkillTargetType.Self,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 10,
                        Cooldown = 10
                    },
                    new Skill
                    {
                        Id = 4,
                        Name = "Skillful Strike",
                        Damage = 20,
                        DamageType = DamageType.Physical,
                        CharacterClass = CharacterClass.Warrior,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    // Mage
                    new Skill
                    {
                        Id = 5,
                        Name = "Arcane Barrier",
                        Damage = 0,
                        DamageType = DamageType.Magic,
                        TargetType = SkillTargetType.Friendly,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 15,
                        Cooldown = 10
                    },
                    new Skill
                    {
                        Id = 6,
                        Name = "Ice Lance",
                        Damage = 20,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    new Skill
                    {
                        Id = 7,
                        Name = "Combustion",
                        Damage = 5,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 10,
                        Cooldown = 3
                    },
                    new Skill
                    {
                        Id = 8,
                        Name = "Lightning Storm",
                        Damage = 10,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Mage,
                        EnergyCost = 30,
                        Cooldown = 10
                    },
                    // Priest
                    new Skill
                    {
                        Id = 9,
                        Name = "Battle Meditation",
                        Damage = 0,
                        DamageType = DamageType.Magic,
                        TargetType = SkillTargetType.Self,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 10,
                        Cooldown = 10
                    },
                    new Skill
                    {
                        Id = 10,
                        Name = "Miraclous Touch",
                        Damage = 0,
                        Healing = 20,
                        DamageType = DamageType.Magic,
                        TargetType = SkillTargetType.Friendly,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 15,
                        Cooldown = 3
                    },
                    new Skill
                    {
                        Id = 11,
                        Name = "Holy Smite",
                        Damage = 20,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 20,
                        Cooldown = 2
                    },
                    new Skill
                    {
                        Id = 12,
                        Name = "Cleansing Pain",
                        Damage = 5,
                        DamageType = DamageType.Magic,
                        CharacterClass = CharacterClass.Priest,
                        EnergyCost = 10,
                        Cooldown = 3
                    }
                );

            modelBuilder
                .Entity<Fight>()
                .HasMany(f => f.Characters)
                .WithOne(c => c.Fight)
                .HasForeignKey(c => c.FightId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder
                .Entity<Item>()
                .HasDiscriminator(i => i.Type)
                .HasValue<Gear>(ItemType.Gear)
                .HasValue<Consumable>(ItemType.Consumable);
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<Gear> Gear { get; set; }
        public DbSet<Consumable> Consumables { get; set; }
    }
}
