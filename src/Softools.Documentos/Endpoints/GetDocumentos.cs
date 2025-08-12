using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Documentos.Extensions;
using Softools.Documentos.Models.Dtos;

namespace Softools.Documentos.Endpoints;

public class GetDocumentos : EndpointWithoutRequest<IEnumerable<DocumentoDto>>
{
    private readonly DocumentosDbContext _context;

    public GetDocumentos(DocumentosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/documentos");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _context.Documentos.ToListAsync(ct);

        await Send.OkAsync(result.Select(d => d.ToDto()), ct);

    }
}