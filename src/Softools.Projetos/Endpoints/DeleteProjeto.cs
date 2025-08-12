using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Models.DTOs;

namespace Softools.Projetos.Endpoints;

public class DeleteProjeto : Endpoint<DeleteProjetoRequest>
{
    private readonly ProjetosDbContext _context;
    private readonly ILogger<DeleteProjeto> _logger;

    public DeleteProjeto(ProjetosDbContext context, ILogger<DeleteProjeto> logger)
    {
        _context = context;
        _logger = logger;
    }

    public override void Configure()
    {
        Delete("/projetos/{id}");
        Description(b => b
            .Produces(204)
            .ProducesProblemFE(404)
            .ProducesProblemFE(400)
            .WithTags("Projetos"));

    }

    public override async Task HandleAsync(DeleteProjetoRequest req, CancellationToken ct)
    {
        var projeto = await _context.Projetos.FindAsync(req.Id);
        
        if (projeto is null)
        {
            _logger.LogWarning("Tentativa de excluir projeto não encontrado. ID: {ProjetoId}", req.Id);
            await Send.NotFoundAsync(ct);
            return;
        }
        
        try
        {
            _context.Projetos.Remove(projeto);
            await _context.SaveChangesAsync(ct);

            _logger.LogInformation("Usuário excluído com sucesso. ID: {ProjetoId}", req.Id);
            await Send.NoContentAsync(ct);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Erro ao excluir usuário. ID: {ProjetoId}", req.Id);
            AddError("Não foi possível excluir o projeto devido a restrições no banco de dados");
            await Send.ErrorsAsync(StatusCodes.Status500InternalServerError, ct);
        }
    }
    
}