using Microsoft.EntityFrameworkCore;
using Softools.Documentos.Entities;

namespace Softools.Documentos;

public class DocumentosDbContext : DbContext
{
    public DocumentosDbContext(DbContextOptions<DocumentosDbContext> options) : base(options)
    {
    }
    
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<TemplateDocumento> Templates { get; set; }
}