using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Entities;

namespace Softools.Projetos.Endpoints;

public class GetProjetos : EndpointWithoutRequest<List<Projeto>>
{
    private readonly ProjetosDbContext _context;

    public GetProjetos(ProjetosDbContext context)
    {
        _context = context;
    }
    
    public override void Configure()
    {
        Get("/projetos");
        Description(b => b
            .Produces<List<Projeto>>(200, "application/json")
            .WithTags("Projetos"));
    }
    
    public override async Task HandleAsync(CancellationToken ct)
    {
        var projetos = await _context.Projetos
            .AsNoTracking()
            .ToListAsync(ct);

        if (!projetos.Any())
        {
            await Send.NoContentAsync(ct);
            return;
        }

        await Send.OkAsync(projetos, cancellation: ct);
    }
    
}