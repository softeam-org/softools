namespace Softools.AppHost;

/// <summary>
/// Uma camada de abstração para garantir que todos os microserviços são configurados de maneira consistente.
/// </summary>
public class Orquestrador
{
    public IDistributedApplicationBuilder Builder { get; private set; }

    private Dictionary<string, IResourceBuilder<ProjectResource>> servicos = new();
    private IResourceBuilder<RabbitMQServerResource> rabbitmq;

    public Orquestrador()
    {
        Builder = DistributedApplication.CreateBuilder();
        
        var rmqUsername = Builder.AddParameter("RabbitMqUsername");
        var rmqPassword = Builder.AddParameter("RabbitMqPassword");
        rabbitmq = Builder.AddRabbitMQ("messaging", rmqUsername, rmqPassword)
            .WithManagementPlugin();
    }

    public Orquestrador AdicionarApi<T>(string nome, Recursos recursos = Recursos.Nenhum, params string[] dependencias)
        where T : IProjectMetadata, new()
    {
        var servico = Builder.AddProject<T>(nome);
        
        // RabbitMQ
        servico.WithReference(rabbitmq)
            .WaitFor(rabbitmq);

        servicos.Add(nome, servico);

        if (recursos.HasFlag(Recursos.BancoDeDadosPostgreSQL))
        {
            ConfigurarPostgres<T>(nome, servico);
        }

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

    private void ConfigurarPostgres<T>(string nome, IResourceBuilder<ProjectResource> servico) where T : IProjectMetadata, new()
    {
        var nomeDb = $"{nome.ToLowerInvariant()}db";
        var postgres = Builder.AddPostgres($"{nome}pg")
            .WithEnvironment("POSTGRES_DB", nomeDb)
            .WithDataVolume(isReadOnly: false);
        var db = postgres.AddDatabase(nomeDb);
        servico.WithReference(db)
            .WaitFor(db);
    }

    public DistributedApplication Build()
    {
        return Builder.Build();
    }
}