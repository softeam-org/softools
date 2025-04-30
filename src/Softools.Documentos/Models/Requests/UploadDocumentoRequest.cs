using Softools.Documentos.ValueTypes;

namespace Softools.Documentos.Models.Requests;

public class UploadDocumentoRequest
{
    public string Nome { get; set; } = string.Empty;
    public TipoDocumento Tipo { get; set; }
    public IFormFile Arquivo { get; set; } = null!;
}