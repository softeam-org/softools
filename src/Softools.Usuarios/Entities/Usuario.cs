namespace Softools.Usuarios.Entities;

public class Usuario
{
    public Usuario(){}
    public Usuario(string nome, string cpf)
    {
        Id = new Guid();
        Nome = nome;
        CPF = cpf;
    }
    public Guid Id { get; init; }
    public string Nome { get; private set; } = string.Empty;
    public string CPF { get; private set; } = string.Empty;
}