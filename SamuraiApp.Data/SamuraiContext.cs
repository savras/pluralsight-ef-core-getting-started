using Microsoft.EntityFrameworkCore;
using SamuraiApp.Domain;

namespace SamuraiApp.Data
{
    public class SamuraiContext : DbContext
    {
        public DbSet<Samurai> Samurais { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Quote> Quotes { get; set; }

        // EF will call this internally.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // We pulled in Microsoft.EntityFrameworkCore.SqlServer
            optionsBuilder.UseSqlServer("Server=(local); Database=SamuraiData; Trusted_Connection=True;");
        }
    }
}