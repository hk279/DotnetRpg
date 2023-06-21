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