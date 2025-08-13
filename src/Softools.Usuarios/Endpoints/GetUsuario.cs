using FastEndpoints;
using Softools.Usuarios.Entities;
using Microsoft.EntityFrameworkCore;
using Softools.Usuarios.Data;

public class GetUsuarioById : Endpoint<GetUsuarioByIdRequest, UsuarioResponse>
{
    private readonly UsuariosDbContext _context;

    public GetUsuarioById(UsuariosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/usuarios/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces<UsuarioResponse>(200, "application/json")
            .ProducesProblem(404, "application/json")
            .WithTags("Usuarios"));
    }

    public override async Task HandleAsync(GetUsuarioByIdRequest req, CancellationToken ct)
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .Where(u => u.Id == req.Id)
            .Select(u => new UsuarioResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                CPF = u.CPF

            })
            .FirstOrDefaultAsync(ct);

        if (usuario is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }

        await Send.OkAsync(usuario, cancellation: ct);
    }
}


