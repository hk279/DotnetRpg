using Microsoft.EntityFrameworkCore;

namespace dotnet_rpg.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "Fireball", Damage = 30, DamageType = DamageType.Magic },
                new Skill { Id = 2, Name = "Charge", Damage = 20, DamageType = DamageType.Physical },
                new Skill { Id = 3, Name = "Backstab", Damage = 40, DamageType = DamageType.Physical }
            );

            modelBuilder.Entity<Character>().HasData(
                new Character(5) { Id = 1, Name = "Wild Boar", IsPlayerCharacter = false, Strength = 10, Intelligence = 0, Armor = 5, Resistance = 5 },
                new Character(5) { Id = 2, Name = "Wolf", IsPlayerCharacter = false, Strength = 5, Intelligence = 0, Armor = 10, Resistance = 5 },
                new Character(5) { Id = 3, Name = "Alpha Wolf", IsPlayerCharacter = false, Strength = 10, Intelligence = 0, Armor = 15, Resistance = 10 }
            );
        }

        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Fight> Fights { get; set; }
    }
}