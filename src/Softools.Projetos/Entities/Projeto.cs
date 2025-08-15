using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices.JavaScript;

namespace Softools.Projetos.Entities;

public class Projeto
{
    public Projeto()
    {
        Id = Guid.NewGuid();
        Status = true;
    }
    
    public Guid Id { get; init; }
    
    public string Nome { get; set; }
    
    public string? Descricao { get; set; }
    public bool Status {get; set;}
    
    public DateOnly DataInicio{get; set;}
    
    public DateOnly DataFim{get; set;}
    
    public string? LinkContrato {get; set;} 
    
}