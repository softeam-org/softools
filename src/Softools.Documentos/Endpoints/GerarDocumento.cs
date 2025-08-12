using FastEndpoints;
using Softools.Documentos.Models.Requests;
using Softools.Documentos.Services;

namespace Softools.Documentos.Endpoints;

public class GerarDocumento : Endpoint<GerarDocumentoRequest>
{
    private readonly DocumentosDbContext _dbContext;
    private readonly TemplateService _templateService;
    
    public GerarDocumento(DocumentosDbContext dbContext, TemplateService templateService)
    {
        _dbContext = dbContext;
        _templateService = templateService;
    }
    
    public override void Configure()
    {
        Post("/documentos/gerar");
        AllowAnonymous();
        Description(x => x
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError));
    }
    
    public override async Task HandleAsync(GerarDocumentoRequest req, CancellationToken ct)
    {
        var template = await _dbContext.Templates.FindAsync(req.TemplateId);
        if (template == null)
        {
            await SendErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var documentoGerado = _templateService.GerarDocumento(template.Caminho, req.Campos);
        if (string.IsNullOrWhiteSpace(documentoGerado))
        {
            await SendErrorsAsync(StatusCodes.Status500InternalServerError, ct);
            return;
        }
        var fileInfo = new FileInfo(documentoGerado);
        
        await SendFileAsync(fileInfo, cancellation: ct);
    }
}