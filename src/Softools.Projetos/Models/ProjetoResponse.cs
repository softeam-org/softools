namespace Softools.Projetos.Models;

public class ProjetoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = String.Empty;
    public string Descricao { get; set; } = String.Empty;
    public bool Status { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}