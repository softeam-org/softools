using FastEndpoints;
using Softools.Documentos.Entities;
using Softools.Documentos.Models.Dtos;
using Softools.Documentos.Models.Requests;

namespace Softools.Documentos.Endpoints;

public class UploadTemplate : Endpoint<UploadTemplateRequest, TemplateDocumentoDto>
{
    private readonly DocumentosDbContext _context;
    
    public UploadTemplate(DocumentosDbContext context)
    {
        _context = context;
    }
    
    public override void Configure()
    {
        Post("/documentos/upload-template");
        AllowAnonymous();
        AllowFileUploads();
        Description(x => x
            .WithName("Upload Template")
            .Produces<TemplateDocumentoDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError));
    }
    
    public override async Task HandleAsync(UploadTemplateRequest req, CancellationToken ct)
    {
        if (req.Arquivo.Length > 0)
        {
            var filePath = Path.Combine(Utils.GetUploadFolderPath(), req.Arquivo.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await req.Arquivo.CopyToAsync(stream, ct);
            }

            var template = new TemplateDocumento
            {
                Nome = req.Nome,
                Caminho = filePath,
                Descricao = req.Descricao
            };

            _context.Templates.Add(template);
            await _context.SaveChangesAsync(ct);

            var response = new TemplateDocumentoDto
            {
                Id = template.Id,
                Nome = template.Nome,
                Caminho = template.Caminho,
                Descricao = template.Descricao
            };

            await Send.OkAsync(response, cancellation: ct);
        }
        else
        {
            await Send.ErrorsAsync(StatusCodes.Status400BadRequest, ct);
        }
    }
    
}