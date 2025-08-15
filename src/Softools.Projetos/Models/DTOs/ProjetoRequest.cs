using Softools.Projetos.Models.Enums;

namespace Softools.Projetos.Models.DTOs;

public class ProjetoRequest
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = String.Empty;
    
    public TipoProjeto Tipo { get; set; }
    public string Descricao { get; set; } = String.Empty;
    
    public StatusProjeto Status  { get; set; } 
    public DateOnly DataInicio { get; set; }
    public DateOnly DataFim { get; set; }
    
    public string? LinkContrato { get; set; }
}