using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Models;
using Softools.Projetos.Models.DTOs;

namespace Softools.Projetos.Endpoints;

public class UpdateProjeto : Endpoint<ProjetoRequest, ProjetoResponse>
{
    private readonly ProjetosDbContext _context;

    public UpdateProjeto(ProjetosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Put("/projetos/{id}");
        Description(b => b
            .Produces<ProjetoResponse>(200)
            .ProducesProblemDetails(400)
            .ProducesProblemFE(404)
            .ProducesProblemFE(409));
    }

    public override async Task HandleAsync(ProjetoRequest req, CancellationToken ct)
    {
        var projeto = await _context.Projetos
            .FirstOrDefaultAsync(u => u.Id == req.Id, ct);
        if (projeto is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        projeto.Nome = req.Nome;
        projeto.Descricao = req.Descricao;
        projeto.DataInicio = req.DataInicio;
        projeto.DataFim = req.DataFim;
        projeto.Status = req.Status;
        projeto.LinkContrato =req.LinkContrato;
        
        await _context.SaveChangesAsync(ct);
        
        var response = new ProjetoResponse
        {
            Id = projeto.Id,
            Nome = projeto.Nome,
            Descricao = projeto.Descricao,
            DataInicio = projeto.DataInicio,
            DataFim = projeto.DataFim,
            Status = projeto.Status,
            LinkContrato =  projeto.LinkContrato
        };

        await Send.OkAsync(response, cancellation: ct);
       
    }
}