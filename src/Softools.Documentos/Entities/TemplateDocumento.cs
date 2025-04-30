namespace Softools.Documentos.Entities;

public class TemplateDocumento
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public string Caminho { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public DateTime DataUltimaAlteracao { get; set; }
    
}