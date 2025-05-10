using Microsoft.EntityFrameworkCore;

namespace Softools.Usuarios.Data;

using Softools.Usuarios.Entities;
public class UsuariosDbContext : DbContext
{
    public DbSet<Usuario> Usuarios { get; set; }
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
        : base(options)
    {
    }
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Usuario>().HasIndex(u => u.Id).IsUnique();
        
    }
    
}