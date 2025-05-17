using System.Net;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Usuarios.Data;
using Softools.Usuarios.Entities;

namespace Softools.Usuarios.Endpoints;

public class UpdateUsuario : Endpoint<UpdateUsuarioRequest, UsuarioResponse>
{
    private readonly UsuariosDbContext _context;

    public UpdateUsuario(UsuariosDbContext context)
        => _context = context;

    public override void Configure()
    {
        Put("/usuarios/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<UsuarioResponse>(200)
            .ProducesProblemDetails(400)
            .ProducesProblemFE(404)
            .ProducesProblemFE(409));
    }

    public override async Task HandleAsync(UpdateUsuarioRequest req, CancellationToken ct)
    {

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == req.Id, ct);

        if (usuario is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }


        if (await _context.Usuarios
            .AnyAsync(u => u.CPF == req.CPF && u.Id != req.Id, ct))
        {
            AddError(r => r.CPF, "CPF já cadastrado por outro usuário");
            await SendErrorsAsync(409, ct);
            return;
        }


        if (req.Nome is not null) usuario.Nome = req.Nome;

        if (req.CPF is not null) usuario.CPF = req.CPF;


        await _context.SaveChangesAsync(ct);


        var response = new UsuarioResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CPF = usuario.CPF
        };

        await SendAsync(response, cancellation: ct);
    }
}

