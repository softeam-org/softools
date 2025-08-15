namespace Softools.Projetos.Models;

public class ProjetoResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = String.Empty;
    public string? Descricao { get; set; }
    public bool Status { get; set; }
    public DateOnly DataInicio { get; set; }
    public DateOnly DataFim { get; set; }
    public string? LinkContrato { get; set; }
}