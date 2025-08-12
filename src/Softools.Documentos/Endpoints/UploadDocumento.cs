using FastEndpoints;
using Softools.Documentos.Entities;
using Softools.Documentos.Models.Dtos;
using Softools.Documentos.Models.Requests;

namespace Softools.Documentos.Endpoints;

public class UploadDocumento : Endpoint<UploadDocumentoRequest, DocumentoDto>
{
    private readonly DocumentosDbContext _dbContext;
    
    public UploadDocumento(DocumentosDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Post("/documentos/upload");
        AllowAnonymous();
        AllowFileUploads();
        Description(x => x
            .Produces<DocumentoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError));
    }
    
    public override async Task HandleAsync(UploadDocumentoRequest req, CancellationToken ct)
    {
        if (req.Arquivo.Length > 0)
        {
            var filePath = Path.Combine(Utils.GetUploadFolderPath(), req.Arquivo.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await req.Arquivo.CopyToAsync(stream, ct);
            }

            var documento = new Documento
            {
                Nome = req.Nome,
                Caminho = filePath,
                Tipo = req.Tipo
            };

            _dbContext.Documentos.Add(documento);
            await _dbContext.SaveChangesAsync(ct);

            var response = new DocumentoDto
            {
                Id = documento.Id,
                Nome = documento.Nome,
                Caminho = documento.Caminho
            };

            await Send.OkAsync(response, cancellation: ct);
        }
        else
        {
            await Send.ErrorsAsync(StatusCodes.Status400BadRequest, ct);
        }
    }
    
}