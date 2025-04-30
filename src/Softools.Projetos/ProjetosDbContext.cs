using Microsoft.EntityFrameworkCore;

namespace Softools.Projetos;

public class ProjetosDbContext : DbContext
{
    public ProjetosDbContext(DbContextOptions<ProjetosDbContext> options) : base(options)
    {
    }
}