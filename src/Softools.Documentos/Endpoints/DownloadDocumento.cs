using FastEndpoints;

namespace Softools.Documentos.Endpoints;

public class DownloadDocumento : EndpointWithoutRequest
{
    private readonly DocumentosDbContext _dbContext;

    public DownloadDocumento(DocumentosDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public override void Configure()
    {
        Get("/documentos/download/{id}");
        AllowAnonymous();
        Description(x => x
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var documento = await _dbContext.Documentos.FindAsync(id);
        
        if (documento == null)
        {
            await Send.ErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        var fileInfo = new FileInfo(documento.Caminho);
        
        if (!fileInfo.Exists)
        {
            await Send.ErrorsAsync(StatusCodes.Status404NotFound, ct);
            return;
        }

        await Send.FileAsync(fileInfo, cancellation: ct);
    }
    
}