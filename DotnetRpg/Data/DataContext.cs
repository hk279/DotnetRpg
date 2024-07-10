using System.Linq.Expressions;
using DotnetRpg.Data.EntityTypeConfigurations;
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
            modelBuilder.ApplyConfiguration(new CharacterEntityTypeConfiguration(GetUserFilter<Character>()));
            modelBuilder.ApplyConfiguration(new FightEntityTypeConfiguration(GetUserFilter<Fight>()));
            modelBuilder.ApplyConfiguration(new ItemEntityTypeConfiguration(GetUserFilter<Item>()));
            modelBuilder.ApplyConfiguration(new SkillInstanceEntityTypeConfiguration(GetUserFilter<SkillInstance>()));
            modelBuilder.ApplyConfiguration(new StatusEffectInstanceEntityTypeConfiguration(GetUserFilter<StatusEffectInstance>()));
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ApplyEntityTypeConfigurations(modelBuilder);
        }
        
        private Expression<Func<TEntity, bool>> GetUserFilter<TEntity>()
            where TEntity : BaseEntity
        {
            return x => x.UserId == UserId;
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
