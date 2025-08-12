using System.Text.RegularExpressions;
using DocumentFormat.OpenXml.Packaging;
using FastEndpoints;
using Softools.Documentos.Models.Requests;

namespace Softools.Documentos.Endpoints;

public class GetCamposTemplate : Endpoint<GetCamposTemplateRequest>
{
    private readonly DocumentosDbContext _context;

    public GetCamposTemplate(DocumentosDbContext context)
    {
        _context = context;
    }

    public override void Configure()
    {
        Get("/templates/campos/{Id}");
        AllowAnonymous();
        Description(x => x
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(GetCamposTemplateRequest req, CancellationToken ct)
    {
        var template = await _context.Templates.FindAsync(req.Id);
        if (template == null)
        {
            await Send.ErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        using var wordDoc = WordprocessingDocument.Open(template.Caminho, true);
        var docPart = wordDoc.MainDocumentPart!;
        var doc = docPart.Document!;
        
        var regex = new Regex(@"\{\{(.*?)\}\}");
        var campos = regex.Matches(doc.InnerXml)
            .Select(m => m.Groups[1].Value);
        
        if (campos == null || !campos.Any())
        {
            await Send.ErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        await Send.OkAsync(campos, cancellation: ct);
    }
}