using FastEndpoints;
using Softools.Documentos.Extensions;
using Softools.Documentos.Models.Dtos;
using Softools.Documentos.Models.Requests;

namespace Softools.Documentos.Endpoints;

public class UpdateTemplate : Endpoint<UpdateTemplateRequest, TemplateDocumentoDto>
{
    private readonly DocumentosDbContext _context;

    public UpdateTemplate(DocumentosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Patch("/templates/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(UpdateTemplateRequest req, CancellationToken ct)
    {
        var template = await _context.Templates.FindAsync(req.Id);

        if (template is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        if(req.Nome is not null) template.Nome = req.Nome;
        if(req.Descricao is not null) template.Descricao = req.Descricao;
        
        await _context.SaveChangesAsync(ct);

        await Send.OkAsync(template.ToDto(), ct);
    }
}