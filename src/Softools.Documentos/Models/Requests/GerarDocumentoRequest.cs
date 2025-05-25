namespace Softools.Documentos.Models.Requests;

public class GerarDocumentoRequest
{
    public int TemplateId { get; set; }
    public Dictionary<string, string> Campos { get; set; } = new();
}