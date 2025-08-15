using Microsoft.EntityFrameworkCore;

namespace Softools.Rubby;

public class RubbyDbContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ConnectionStrings__rubbydb"));
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.ConfigKey>()
            .HasKey(c => c.Key);

        modelBuilder.Entity<Entities.ConfigKey>()
            .Property(c => c.Value)
            .IsRequired(false);
    }

    public DbSet<Entities.ConfigKey> ConfigKeys { get; set; }
    
}