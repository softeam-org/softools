namespace Softools.Documentos.Models.Requests;

public class GerarDocumentoRequest
{
    public int TemplateId { get; set; }
    public string NomeDisplay { get; set; } = string.Empty;
    public Dictionary<string, string> Campos { get; set; } = new();
}