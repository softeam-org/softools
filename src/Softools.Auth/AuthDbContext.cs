using Microsoft.EntityFrameworkCore;
using Softools.Auth.Entities;

namespace Softools.Auth;

public class AuthDbContext : DbContext
{
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
    {
    }

    public DbSet<UserCredentials> UserCredentials { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserCredentials>()
            .HasKey(uc => uc.Id);

        modelBuilder.Entity<UserCredentials>().HasIndex(c => c.Email).IsUnique();

        modelBuilder.Entity<Role>()
            .HasKey(r => r.Id);

        modelBuilder.Entity<UserRole>()
            .HasKey(ur => new { ur.UserId, ur.RoleId });

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);

        modelBuilder.Entity<Role>()
            .HasData(
                new Role() { Name = "Presidente"},
                new Role() { Name = "Diretor" },
                new Role() { Name = "Projetos" },
                new Role() { Name = "RH" },
                new Role() { Name = "Financeiro" },
                new Role() { Name = "Marketing" },
                new Role() { Name = "Comercial" }
            );
    }
}