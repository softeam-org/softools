namespace Softools.AppHost;

/// <summary>
/// Representa recursos que serão alocados para cada microserviço.
/// </summary>
[Flags]
public enum Recursos
{
    Nenhum = 0,
    BancoDeDadosPostgreSQL = 1,
}