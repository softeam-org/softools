using System.Net;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Usuarios.Data;
using Softools.Usuarios.Entities;

namespace Softools.Usuarios.Endpoints;

public class CreateUsuario : Endpoint<UsuarioRequest, UsuarioResponse>
{
    private readonly UsuariosDbContext _context;

    public CreateUsuario(UsuariosDbContext context)
        => _context = context;

    public override void Configure()
    {
        Post("/usuarios");
        AllowAnonymous();
        Description(b=>b
        .Produces<UsuarioResponse>(201)
        .ProducesProblemDetails(400)
        .ProducesProblemFE(409));
    }

    public override async Task HandleAsync(UsuarioRequest req, CancellationToken ct)
    {
        if (await _context.Usuarios.AnyAsync(u => u.CPF == req.CPF, ct))
        {
            AddError(r => r.CPF, "CPF já cadastrado");
            ThrowIfAnyErrors();
        }
        var usuario = new Usuario
        {
            Nome = req.Nome,
            CPF = req.CPF
        };

        await _context.Usuarios.AddAsync(usuario, ct);
        await _context.SaveChangesAsync(ct);

        var response = new UsuarioResponse
        {
            Id = usuario.Id,
            Nome = usuario.Nome,
            CPF = usuario.CPF
        };

        await SendAsync(response, (int)HttpStatusCode.Created,ct);
        
    }


}