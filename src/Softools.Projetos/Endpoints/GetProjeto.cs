using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Models;
using Softools.Projetos.Models.DTOs;

namespace Softools.Projetos.Endpoints;

public class GetProjeto : Endpoint<GetProjetoByIdRequest ,ProjetoResponse>
{
    private readonly ProjetosDbContext _context;

    public GetProjeto(ProjetosDbContext context)
    {
        _context = context;
    }
    
    public override void Configure()
    {
        Get("/projetos/{id}");
        Description(b => b
            .Produces<ProjetoResponse>(200, "application/json")
            .ProducesProblem(404, "application/json")
            .WithTags("Projetos"));
    }

    public override async Task HandleAsync(GetProjetoByIdRequest req, CancellationToken ct)
    {
        var projeto = await _context.Projetos
            .AsNoTracking()
            .Where(u => u.Id == req.Id)
            .Select(u => new ProjetoResponse
            {
                Id = u.Id,
                Nome = u.Nome,
                Descricao = u.Descricao,
                Status = u.Status,
                DataInicio = u.DataInicio,
                DataFim = u.DataFim,

            })
            .FirstOrDefaultAsync(ct);

        if (projeto is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        await Send.OkAsync(projeto, cancellation: ct);
    }

}