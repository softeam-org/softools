using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Entities;

namespace Softools.Projetos.Data;

public class ProjetosDbContext : DbContext
{
    public DbSet<Projeto> Projetos { get; set; }
    public ProjetosDbContext(DbContextOptions<ProjetosDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Projeto>().HasIndex(p => p.Id).IsUnique();
        builder.Entity<Projeto>().HasIndex(p => p.Nome).IsUnique();
    }
    
}