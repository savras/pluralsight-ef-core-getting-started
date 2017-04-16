using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<SamuraiBattle> SamuraiBattles { get; set; }

        // EF will call these internally.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SamuraiBattle>()
                .HasKey(s => new {s.BattleId, s.SamuraiId});

            // Tell EF that this relationship is mandatory.
            //modelBuilder.Entity<Samurai>()
            //    .Property(s => s.SecretIdentity).IsRequired();

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We pulled in Microsoft.EntityFrameworkCore.SqlServer
            optionsBuilder.UseSqlServer("Server=(local); Database=SamuraiData; Trusted_Connection=True;",
                options => options.MaxBatchSize(30));

            optionsBuilder.EnableSensitiveDataLogging();
        }
    }
}