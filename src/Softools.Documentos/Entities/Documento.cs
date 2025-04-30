using Softools.Documentos.ValueTypes;

namespace Softools.Documentos.Entities;

public class Documento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public TipoDocumento Tipo { get; set; }
}