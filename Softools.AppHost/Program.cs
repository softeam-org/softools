using Projects;
using Softools.AppHost;

var orquestrador = new Orquestrador();

orquestrador.AdicionarApi<Softools_Projetos>("projetos", Recursos.BancoDeDadosPostgreSQL)
    .AdicionarApiGateway<Softools_Gateway>("gateway");

await orquestrador.Build().RunAsync();
