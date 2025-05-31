using System.Linq.Expressions;
using DotnetRpg.Data.EntityTypeConfigurations;
using DotnetRpg.Models.Characters;
using DotnetRpg.Models.Fights;
using DotnetRpg.Models.Generic;
using DotnetRpg.Models.Items;
using DotnetRpg.Models.Skills;
using DotnetRpg.Models.Users;
using DotnetRpg.Services.UserProvider;
using Microsoft.EntityFrameworkCore;

namespace DotnetRpg.Data
{
    public class DataContext : DbContext
    {
        private readonly IUserProvider _userProvider;
        
        public DataContext(DbContextOptions<DataContext> options, IUserProvider userProvider)
            : base(options)
        {
            _userProvider = userProvider;
        }
        
        private void ApplyEntityTypeConfigurations(ModelBuilder modelBuilder)
        {
            // User-entity (tenant)
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            
            // Global entities, not user-specific
            modelBuilder.ApplyConfiguration(new SkillEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StatusEffectEntityTypeConfiguration());
            
            // User-specific entities
            modelBuilder.ApplyConfiguration(new CharacterEntityTypeConfiguration(GetCharacterGlobalQueryFilter()));
            
            // Character-specific entities
            modelBuilder.ApplyConfiguration(new FightEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ItemEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SkillInstanceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new StatusEffectInstanceEntityTypeConfiguration());
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyEntityTypeConfigurations(modelBuilder);
        }
        
        private Expression<Func<Character, bool>> GetCharacterGlobalQueryFilter()
        {
            return c => c.UserId == UserId || c.IsPlayerCharacter;
        }
        
        public int UserId => _userProvider.GetUserId();
        public DbSet<Character> Characters { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Fight> Fights { get; set; }
        public DbSet<ArmorPiece> ArmorPieces { get; set; }
        public DbSet<Weapon> Weapons { get; set; }
    }
}
