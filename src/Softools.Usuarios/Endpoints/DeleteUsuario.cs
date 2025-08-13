using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Softools.Usuarios.Data;
using Softools.Usuarios.Entities;
using System.Net;

namespace Softools.Usuarios.Endpoints;

public class DeleteUsuario : Endpoint<DeleteUsuarioRequest>
{
    private readonly UsuariosDbContext _context;
    private readonly ILogger<DeleteUsuario> _logger;

    public DeleteUsuario(UsuariosDbContext context, ILogger<DeleteUsuario> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override void Configure()
    {
        Delete("/usuarios/{id}");
        AllowAnonymous();
        Description(b => b
            .Produces(204)
            .ProducesProblemFE(404)
            .ProducesProblemFE(400)
            .WithTags("Usuarios"));


    }

    public override async Task HandleAsync(DeleteUsuarioRequest req, CancellationToken ct)
    {

        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == req.Id, ct);

        if (usuario is null)
        {
            _logger.LogWarning("Tentativa de excluir usuário não encontrado. ID: {UsuarioId}", req.Id);
            await Send.NotFoundAsync(ct);
            return;
        }

        try
        {
            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Usuário excluído com sucesso. ID: {UsuarioId}", req.Id);
            await Send.NoContentAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário. ID: {UsuarioId}", req.Id);
            AddError("Não foi possível excluir o usuário devido a restrições no banco de dados");
            await Send.ErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
}

