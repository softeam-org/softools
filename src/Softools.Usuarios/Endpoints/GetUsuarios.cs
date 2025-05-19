using FastEndpoints;
using Softools.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Softools.Usuarios.Data;

public class GetUsuarios : EndpointWithoutRequest<List<Usuario>>
{
    private readonly UsuariosDbContext _context;

    public GetUsuarios(UsuariosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/usuarios");
        AllowAnonymous();
        Description(b => b
            .Produces<List<Usuario>>(200, "application/json")
            .WithTags("Usuarios"));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var usuarios = await _context.Usuarios
            .AsNoTracking()
            .ToListAsync(ct);

        if (!usuarios.Any())
        {
            await SendNoContentAsync(ct);
            return;
        }

        await SendAsync(usuarios, cancellation: ct);
    }
}