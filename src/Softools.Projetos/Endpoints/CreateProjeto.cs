using System.Net;
using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Projetos.Data;
using Softools.Projetos.Entities;
using Softools.Projetos.Models;
using Softools.Projetos.Models.DTOs;

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
        
        var projeto = new Projeto
        {
            Nome = req.Nome,
            Tipo = req.Tipo,
            Descricao = req.Descricao,
            DataInicio = req.DataInicio,
            DataFim = req.DataFim,
            LinkContrato = req.LinkContrato
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
            Status = projeto.Status,
            LinkContrato = projeto.LinkContrato
        };
        
        await SendAsync(response, (int)HttpStatusCode.Created,ct);
    }
}