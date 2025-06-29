namespace Softools.Projetos.Entities;

public class Projeto
{
    public Projeto()
    {
        Id = new Guid();
        Status = true;
    }
    
    public Guid Id { get; init; }
    
    public string Nome { get; set; } = String.Empty;
    
    public string Descricao { get; set; } = String.Empty;
    public bool Status {get; set;}

    public DateTime DataInicio{get; set;}

    public DateTime DataFim{get; set;}
    
}