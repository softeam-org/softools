namespace Softools.Documentos.Models.Requests;

public class UploadTemplateRequest
{
    public string Nome { get; set; } = string.Empty;
    public IFormFile Arquivo { get; set; } = null!;
    public string Descricao { get; set; } = string.Empty;
}