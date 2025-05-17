using FastEndpoints;
using Softools.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Softools.Usuarios.Data;

public class GetAllUsuarios : EndpointWithoutRequest<List<Usuario>>
{
    private readonly UsuariosDbContext _context;

    public GetAllUsuarios(UsuariosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/usuarios");
        Description(b => b
            .Produces<List<Usuario>>(200, "application/json") 
            .WithTags("Usuarios")
            .RequireAuthorization());
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var usuarios = await _context.Usuarios
            .AsNoTracking() // Recomendado para operações somente leitura
            .ToListAsync(ct);

        if (!usuarios.Any())
        {
            await SendNoContentAsync(ct); // 204 se vazio
            return;
        }

        await SendAsync(usuarios, cancellation: ct);
    }
}