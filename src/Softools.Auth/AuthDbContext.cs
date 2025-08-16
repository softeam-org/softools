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

        modelBuilder.Entity<Role>().HasData(
            new Role { Id = Guid.Parse("11111111-1111-1111-1111-111111111111"), Name = "Presidente" },
            new Role { Id = Guid.Parse("22222222-2222-2222-2222-222222222222"), Name = "Diretor" },
            new Role { Id = Guid.Parse("33333333-3333-3333-3333-333333333333"), Name = "Projetos" },
            new Role { Id = Guid.Parse("44444444-4444-4444-4444-444444444444"), Name = "RH" },
            new Role { Id = Guid.Parse("55555555-5555-5555-5555-555555555555"), Name = "Financeiro" },
            new Role { Id = Guid.Parse("66666666-6666-6666-6666-666666666666"), Name = "Marketing" },
            new Role { Id = Guid.Parse("77777777-7777-7777-7777-777777777777"), Name = "Comercial" }
        );

    }
}