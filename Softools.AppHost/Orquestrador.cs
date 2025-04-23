namespace Softools.AppHost;

/// <summary>
/// Uma camada de abstração para garantir que todos os microserviços são configurados de maneira consistente.
/// </summary>
public class Orquestrador
{
    
    /// <summary>
    /// Builder da aplicação distribuída, usado para registrar e configurar recursos e serviços.
    /// </summary>
    public IDistributedApplicationBuilder Builder { get; private set; }

    private Dictionary<string, IResourceBuilder<ProjectResource>> servicos = new();
    private IResourceBuilder<RabbitMQServerResource> rabbitmq;
    private IResourceBuilder<ProjectResource> gateway;

    public Orquestrador()
    {
        Builder = DistributedApplication.CreateBuilder();
        
        var rmqUsername = Builder.AddParameter("RabbitMqUsername");
        var rmqPassword = Builder.AddParameter("RabbitMqPassword");
        rabbitmq = Builder.AddRabbitMQ("messaging", rmqUsername, rmqPassword)
            .WithManagementPlugin();
    }

    /// <summary>
    /// Adiciona uma API ao orquestrador, com configuração opcional de banco de dados e dependências.
    /// </summary>
    /// <typeparam name="T">Tipo que implementa <see cref="IProjectMetadata"/>.</typeparam>
    /// <param name="nome">Nome do serviço.</param>
    /// <param name="recursos">Recursos opcionais a serem configurados, como banco de dados PostgreSQL.</param>
    /// <param name="dependencias">Lista de nomes de serviços dos quais esta API depende.</param>
    /// <returns>Instância atual de <see cref="Orquestrador"/> para encadeamento.</returns>
    /// <exception cref="InvalidOperationException">Lançada quando uma dependência informada não foi registrada.</exception>
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
    
    /// <summary>
    /// Adiciona o API Gateway ao orquestrador.
    /// </summary>
    /// <typeparam name="T">Tipo que implementa <see cref="IProjectMetadata"/>.</typeparam>
    /// <param name="nome">Nome do gateway.</param>
    /// <returns>Instância atual de <see cref="Orquestrador"/> para encadeamento.</returns>
    public Orquestrador AdicionarApiGateway<T>(string nome) where T : IProjectMetadata, new()
    {
        gateway = Builder.AddProject<T>(nome);
        
        // RabbitMQ
        gateway.WithReference(rabbitmq)
            .WaitFor(rabbitmq);

        return this;
    }

    /// <summary>
    /// Constrói e retorna a aplicação distribuída configurada.
    /// </summary>
    /// <returns>Instância de <see cref="DistributedApplication"/>.</returns>
    /// <exception cref="InvalidOperationException">Lançada se o API Gateway não tiver sido configurado.</exception>
    public DistributedApplication Build()
    {
        if (gateway == null) 
        {
            throw new InvalidOperationException("Gateway not set up");
        }

        foreach (var servico in servicos.Values)
        {
            gateway.WithReference(servico)
                .WaitFor(servico);
        }
        return Builder.Build();
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

}