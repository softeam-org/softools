using Microsoft.EntityFrameworkCore;

namespace Softools.Usuarios;

public class UsuariosDbContext : DbContext
{
    public UsuariosDbContext(DbContextOptions<UsuariosDbContext> options)
        : base(options)
    {
    }
    
}