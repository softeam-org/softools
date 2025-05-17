using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Softools.Documentos.Extensions;
using Softools.Documentos.Models.Dtos;

namespace Softools.Documentos.Endpoints;

public class GetTemplates : EndpointWithoutRequest<IEnumerable<TemplateDocumentoDto>>
{
    private readonly DocumentosDbContext _context;

    public GetTemplates(DocumentosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/templates");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result = await _context.Templates.ToListAsync(ct);
        await SendOkAsync(result.Select(t => t.ToDto()), ct);
    }
}