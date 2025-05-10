using FastEndpoints;
using Softools.Usuarios.Data;
using Softools.Usuarios.Entities;

namespace Softools.Usuarios.Endpoints;

public class CreateUsuario : Endpoint<UsuarioRequest, Usuario>
{
    private readonly UsuariosDbContext _context;

    public CreateUsuario(UsuariosDbContext context)
        => _context = context;

    public override void Configure()
    {
        Post("/usuarios");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UsuarioRequest req, CancellationToken ct)
    {
        var usuario = new Usuario
        {
            Nome = req.Nome,
            CPF = req.CPF
        };

        await _context.Usuarios.AddAsync(usuario, ct);
        await _context.SaveChangesAsync(ct);

        await SendAsync(usuario, cancellation: ct);
        
    }


}