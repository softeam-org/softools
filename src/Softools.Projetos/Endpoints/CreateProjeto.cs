using System.Net;
using FastEndpoints;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Entities;
using Softools.Projetos.Models;

namespace Softools.Projetos.Endpoints;

public class CreateProjeto : Endpoint<ProjetoRequest, ProjetoResponse>
{
    private readonly ProjetosDbContext _context;
    public CreateProjeto(ProjetosDbContext context)
        => _context = context;
    public override void Configure()
    {
        Post("/projetos");
        AllowAnonymous();
        Description(b => b
            .Produces<ProjetoResponse> (201)
            .ProducesProblemDetails(400)
            .ProducesProblemFE(409));
    }
    
    
    public override async Task HandleAsync(ProjetoRequest req,CancellationToken ct)
    {
        if (await _context.Projetos.AnyAsync(u => u.Nome == req.Nome, ct))
        {
            AddError(r => r.Nome, "Nome já cadastrado");
            ThrowIfAnyErrors();
        }
        
        var projeto = new Projeto
        {
            Nome = req.Nome,
            Descricao = req.Descricao,
            DataInicio = req.DataInicio,
            DataFim = req.DataFim
        };
        
        await _context.Projetos.AddAsync(projeto, ct);
        await _context.SaveChangesAsync(ct);
        
        var response = new ProjetoResponse
        {
            Id = projeto.Id,
            Nome = projeto.Nome,
            Descricao = projeto.Descricao,
            DataInicio = projeto.DataInicio,
            DataFim = projeto.DataFim,
            Status = projeto.Status
        };
        
        await SendAsync(response, (int)HttpStatusCode.Created,ct);
    }
}