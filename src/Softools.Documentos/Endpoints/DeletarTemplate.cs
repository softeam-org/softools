using FastEndpoints;
using Softools.Documentos.Models.Requests;

namespace Softools.Documentos.Endpoints;

public class DeletarTemplate : Endpoint<DeleteTemplateRequest>
{
    private readonly DocumentosDbContext _context;

    public DeletarTemplate(DocumentosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Delete("/templates/{Id}");
        AllowAnonymous();
    }

    public override async Task HandleAsync(DeleteTemplateRequest req, CancellationToken ct)
    {
        var template = await _context.Templates.FindAsync(req.Id);

        if (template is null)
        {
            await Send.NotFoundAsync(ct);
            return;
        }
        
        _context.Templates.Remove(template);
        await _context.SaveChangesAsync(ct);

        await Send.NoContentAsync(ct);
    }
}