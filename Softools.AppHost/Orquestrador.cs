namespace Softools.AppHost;

/// <summary>
/// Uma camada de abstração para garantir que todos os microserviços são configurados de maneira consistente.
/// </summary>
public class Orquestrador
{
    public IDistributedApplicationBuilder Builder { get; private set; }

    private Dictionary<string, IResourceBuilder<ProjectResource>> servicos = new();

    public Orquestrador()
    {
        Builder = DistributedApplication.CreateBuilder();
    }

    public Orquestrador AdicionarApi<T>(string nome, Recursos recursos = Recursos.Nenhum, params string[] dependencias)
        where T : IProjectMetadata, new()
    {
        var servico = Builder.AddProject<T>(nome);

        servicos.Add(nome, servico);

        foreach (var dependencia in dependencias)
        {
            if (!servicos.ContainsKey(dependencia))
            {
                throw new InvalidOperationException($"Dependencia '{dependencia}' não encontrada");
            }

            servico.WithReference(servicos[dependencia])
                .WaitFor(servicos[dependencia]);
        }

        return this;
    }

    public DistributedApplication Build()
    {
        return Builder.Build();
    }
}