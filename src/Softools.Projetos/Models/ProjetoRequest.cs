namespace Softools.Projetos.Models;

public class ProjetoRequest
{
    public string Nome { get; set; } = String.Empty;
    public string Descricao { get; set; } = String.Empty;
    public bool Status { get; set; }
    public DateTime DataInicio { get; set; }
    public DateTime DataFim { get; set; }
}